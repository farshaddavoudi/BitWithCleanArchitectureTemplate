using Autofac;
using Bit.Core.Contracts;
using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.Infrastructure.ConfigureServices;

/// <summary>
/// Register Assembly Public NonAbstract Classes with PropertyInjection enabled
/// </summary>
public class NamePatternRegisterInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        dependencyManager
            .GetContainerBuilder()
            .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .PublicOnly()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                (type.Name.EndsWith("Service") || type.Name.EndsWith("Repository")) &&
                type.Name != "EntityService")
            .AsSelf()
            .As(t => t.BaseType!)
            .AsImplementedInterfaces()
            .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues)
            .PreserveExistingDefaults();
    }
}