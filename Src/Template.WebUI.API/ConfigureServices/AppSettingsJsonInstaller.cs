using Bit.Core.Contracts;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;

namespace Template.WebUI.API.ConfigureServices;

public class AppSettingsJsonInstaller : IBitInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        // Register (Server)AppSettings as Singleton to easy use
        dependencyManager.RegisterInstance(appSettings);
    }
}