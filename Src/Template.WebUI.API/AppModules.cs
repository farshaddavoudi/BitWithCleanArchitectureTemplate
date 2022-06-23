using ATABit.Server;
using ATABit.Shared.Extensions;
using Bit.Core.Contracts;
using Bit.Model.Implementations;
using Bit.Owin.Contracts;
using Bit.Owin.Implementations;
using Bit.WebApi.Implementations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Microsoft.Owin.Cors;
using Owin;
using Swashbuckle.Application;
using System.IO.Compression;
using System.Reflection;
using Template.Domain.Options;
using Template.Domain.Shared;
using Template.Infrastructure;
using Template.Infrastructure.Common.Contracts;
using Template.Infrastructure.Identity;
using Template.WebUI.API.ActionFilters;
using Template.WebUI.API.Extensions;

namespace Template.WebUI.API;

public class AppModules : IAppModule
{
    private static bool IsDevelopment() => AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsDevelopment();

    public void ConfigureDependencies(IServiceCollection services, IDependencyManager dependencyManager)
    {
        ConfigureServices(services, dependencyManager);

        ConfigureMiddleware(dependencyManager);
    }

    private void ConfigureServices(IServiceCollection services, IDependencyManager dependencyManager)
    {
        dependencyManager.RegisterMinimalDependencies();

        dependencyManager.RegisterDefaultAspNetCoreApp();

        dependencyManager.RegisterDefaultWebApiAndODataConfiguration();

        dependencyManager.Register<IExceptionToHttpErrorMapper, ATAExceptionToHttpErrorMapper>();

        dependencyManager
            .RegisterWebApiConfigurationCustomizer<GlobalDefaultExceptionHandlerActionFilterProvider<
                ATAExceptionHandlerFilterAttribute>>();

        dependencyManager.RegisterModelStateValidator();

        dependencyManager.RegisterDtoEntityMapper();
        dependencyManager.RegisterMapperConfiguration<DefaultMapperConfiguration>();
        dependencyManager.RegisterAutoMapperConfigurations(new[] { typeof(APIAssemblyEntryPoint).Assembly, typeof(InfrastructureAssemblyEntryPoint).Assembly });

        services.AddRazorPages().AddApplicationPart(typeof(AppStartup).Assembly);
        services.AddControllers(options =>
        {
            options.Filters.Add(new ATAAuthorizeAttribute());
            options.Conventions.Add(new RouteTokenTransformerConvention(new CamelCaseDasherizeParameterTransformer()));
        }).AddApplicationPart(typeof(APIAssemblyEntryPoint).Assembly);

        services.AddResponseCompression(opts =>
        {
            opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Where(m => m != "text/html").Concat(new[] { "application/octet-stream" }).ToArray();
            opts.EnableForHttps = true;
            opts.Providers.Add<BrotliCompressionProvider>();
            opts.Providers.Add<GzipCompressionProvider>();
        })
            .Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest)
            .Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = AppMetadata.PersianFullName, Version = "v1" });
        });

        // Configure Dependencies with Service Installers 
        var serverAppSettings = AspNetCoreAppEnvironmentsProvider.Current.Configuration.GetSection(nameof(ServerAppSettings)).Get<ServerAppSettings>();

        var appSettings = new AppSettings { Server = serverAppSettings };

        services.AddApplicationServices(appSettings);

        var assemblies = new[] {
            typeof(APIAssemblyEntryPoint).Assembly,
            typeof(InfrastructureAssemblyEntryPoint).Assembly
        };

        var bitInstallers = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IBitInstaller).IsAssignableFrom(c))
            .Select(Activator.CreateInstance).Cast<IBitInstaller>().ToList();

        bitInstallers.ForEach(i => i.InstallServices(services, dependencyManager, appSettings));
    }

    private void ConfigureMiddleware(IDependencyManager dependencyManager)
    {
        dependencyManager.RegisterAspNetCoreMiddlewareUsing(aspNetCoreApp =>
        {
#if BlazorClient
            if (AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsDevelopment())
                aspNetCoreApp.UseWebAssemblyDebugging();
            aspNetCoreApp.UseBlazorFrameworkFiles();
#endif

            // Global CORS Policy
            aspNetCoreApp.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            aspNetCoreApp.UseRequestLocalization();

            aspNetCoreApp.UseResponseCompression();
            aspNetCoreApp.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true
                    };
                }
            });

            aspNetCoreApp.UseRouting();

            aspNetCoreApp.UseSwagger();
            aspNetCoreApp.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppMetadata.EnglishFullName} v1"));
        });

        dependencyManager.RegisterMinimalAspNetCoreMiddlewares();

        dependencyManager.RegisterAspNetCoreSingleSignOnClient();

        dependencyManager.RegisterODataMiddleware(odataDependencyManager =>
        {
            odataDependencyManager.RegisterGlobalWebApiCustomizerUsing(httpConfiguration =>
            {
                httpConfiguration.Filters.Add(new ATAAuthorizeAttribute());
                httpConfiguration.EnableSwagger(c =>
                {
                    var xmlDocs = new[] { Assembly.GetExecutingAssembly() }
                        .Select(a => Path.Combine(Path.GetDirectoryName(a.Location)!, $"{a.GetName().Name}.xml"))
                        .Where(File.Exists).ToArray();
                    c.SingleApiVersion("v1", $"Swagger-Api");
                    Array.ForEach(xmlDocs, c.IncludeXmlComments);
                    c.ApplyDefaultODataConfig(httpConfiguration);
                }).EnableBitSwaggerUi();
            });

            odataDependencyManager.RegisterWebApiODataMiddlewareUsingDefaultConfiguration();
        });

        dependencyManager.RegisterSingleSignOnServer<IdentityUserService, IdentityClientsProvider>();

        dependencyManager.RegisterAspNetCoreMiddlewareUsing(aspNetCoreApp =>
        {
#if BlazorClient
            #region Route Checkings

            //aspNetCoreApp.MapWhen(context => context.Request.Path == "/", innerAspNetCoreApp =>
            //{
            //    innerAspNetCoreApp.Run(async context =>
            //    {
            //        #region Route base Checks

            //        //if (context.Request.Path == "/")
            //        //{
            //        //    //IUserInformationProvider userInformationProvider = context.RequestServices.GetRequiredService<IUserInformationProvider>();

            //        //    //if (!userInformationProvider.IsAuthenticated())
            //        //    //{
            //        //    //    context.Response.Redirect("/login");
            //        //    //}

            //        //    #region Redirect to some page based on user claims
            //        //    //else
            //        //    //{
            //        //    //    BitJwtToken jwtToken = userInformationProvider.GetBitJwtToken();

            //        //    //    if (jwtToken.Claims.ContainsKey(RedemptionStrings.BusinessId))
            //        //    //    {
            //        //    //        context.Response.Redirect("/home");
            //        //    //    }
            //        //    //    else
            //        //    //    {
            //        //    //        context.Response.Redirect("/select-business");
            //        //    //    }
            //        //    //} 
            //        //    #endregion
            //        //}

            //        #endregion
            //    });
            //}); 

            #endregion
#endif

            aspNetCoreApp.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
#if BlazorClient
                endpoints.MapFallbackToPage("/_Host");
#endif
            });
        }, MiddlewarePosition.AfterOwinMiddlewares);

        // For Telerik file uploads ajax request in case of blazor server && 3rd party Swagger UI!
        dependencyManager.RegisterOwinMiddlewareUsing(owinApp =>
        {
            owinApp.UseCors(CorsOptions.AllowAll);
        });
    }
}