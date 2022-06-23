using Autofac;
using Bit.Core.Contracts;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;

namespace Template.WebUI.API.ConfigureServices
{
    /// <summary>
    /// Register Assembly Public NonAbstract Classes with PropertyInjection enabled
    /// </summary>
    public class NamePatternRegisterInstaller : IBitInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            dependencyManager
                .GetContainerBuilder()
                .RegisterAssemblyTypes(typeof(APIAssemblyEntryPoint).Assembly)
                .PublicOnly()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    type.Name.EndsWith("Service") &&
                    type.Name != "EntityService")
                .AsSelf()
                .As(t => t.BaseType!)
                .AsImplementedInterfaces()
                .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
                .PreserveExistingDefaults();
        }
    }
}