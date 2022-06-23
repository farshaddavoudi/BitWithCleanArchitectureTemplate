using Autofac;
using NetCore.AutoRegisterDi;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.Application.ConfigureServices.Installers;

/// <summary>
/// Register Assembly Public NonAbstract Classes 
/// </summary>
public class AutoRegisterInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, AppSettings appSettings)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))  //optional
        //    .IgnoreThisInterface<IMyInterface>()     //optional
              .AsPublicImplementedInterfaces();

        #region Scanning Multiple assemblies example

        //var assembliesToScan = new[]
        //{
        //    Assembly.GetExecutingAssembly(),
        //    Assembly.GetAssembly(typeof(MyServiceInAssembly1)),
        //    Assembly.GetAssembly(typeof(MyServiceInAssembly2))
        //};

        //services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
        //    .Where(c => c.Name.EndsWith("Service"))  //optional
        //    .IgnoreThisInterface<IMyInterface>()     //optional
        //    .AsPublicImplementedInterfaces();

        #endregion
    }
}