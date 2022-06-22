using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Application.Common.Implementations;
using Template.Domain.Options;

namespace Template.Application.ConfigureServices;

public class LocalizationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        //services.AddLocalization();

        //services.AddRequestLocalization(options =>
        //{
        //    var supportedCultures = new[]
        //    {
        //        new CultureInfo("fa"),
        //        new CultureInfo("en"),
        //    };

        //    options.SupportedCultures = supportedCultures;
        //    options.DefaultRequestCulture = new RequestCulture("fa");
        //    options.ApplyCurrentCultureToResponseHeaders = true;
        //    options.SupportedUICultures = supportedCultures;
        //    options.RequestCultureProviders = new List<IRequestCultureProvider>
        //    {
        //        new QueryStringRequestCultureProvider(),
        //        new AcceptLanguageHeaderRequestCultureProvider()
        //        // new CookieRequestCultureProvider()
        //    };
        //});

        dependencyManager.Register<IStringProvider, StringProvider>();

        //services.AddSingleton<IConfigureOptions<MvcOptions>, MvcConfigurationToProvideModelBindingMessage>();
    }
}