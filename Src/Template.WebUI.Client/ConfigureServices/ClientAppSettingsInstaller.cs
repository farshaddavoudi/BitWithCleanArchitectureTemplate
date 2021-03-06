using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices;

public class ClientAppSettingsInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, AppSettings appSettings)
    {
        // Directly with injecting ClientAppSettings class
        services.AddSingleton(serviceProvider => appSettings);
    }
}