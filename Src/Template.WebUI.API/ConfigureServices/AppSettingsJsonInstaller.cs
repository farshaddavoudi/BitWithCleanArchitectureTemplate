using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.API.ConfigureServices;

public class AppSettingsJsonInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        // Register (Server)AppSettings as Singleton to easy use
        dependencyManager.RegisterInstance(appSettings);
    }
}