using Blazored.Toast;
using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Template.WebUI.Client.Contracts;
using Template.WebUI.Client.Implementations;

namespace Template.WebUI.Client.ConfigureServices;

public class NotificationInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, AppSettings appSettings)
    {
        //https://github.com/Blazored/Toast
        services.AddBlazoredToast();

        services.AddTransient<INotificationService, NotificationService>();
    }
}