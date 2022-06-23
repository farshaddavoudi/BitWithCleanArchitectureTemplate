using Blazored.LocalStorage;
using Template.Application.Common.Contracts;
using Template.Domain.Options;


namespace Template.WebUI.Client.ConfigureServices
{
    public class LocalStorageInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // https://github.com/Blazored/LocalStorage
            services.AddBlazoredLocalStorage();
        }
    }
}