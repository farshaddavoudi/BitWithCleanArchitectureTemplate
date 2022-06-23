// ReSharper disable CheckNamespace

using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings)
    {
        var installers = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IInstaller).IsAssignableFrom(c))
            .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

        installers.ForEach(i => i.InstallServices(services, appSettings));

        return services;
    }
}