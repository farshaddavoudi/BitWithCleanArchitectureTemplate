using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Template.WebUI.Client.Models;

namespace Template.WebUI.Client.ConfigureServices
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // Singleton cash using by directly injecting AppData into a class
            var appCache = new AppData();

            services.AddSingleton(sp => appCache);

            //services.AddTransient<IAppDataService, AppDataService>();
        }
    }
}