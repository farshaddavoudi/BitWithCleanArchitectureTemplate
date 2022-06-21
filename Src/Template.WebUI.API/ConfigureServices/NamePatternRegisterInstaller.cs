using Autofac;
using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.API.ConfigureServices
{
    /// <summary>
    /// Register Assembly Public NonAbstract Classes with PropertyInjection enabled
    /// </summary>
    public class NamePatternRegisterInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            dependencyManager
                .GetContainerBuilder()
                .RegisterAssemblyTypes(typeof(Program).Assembly)
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