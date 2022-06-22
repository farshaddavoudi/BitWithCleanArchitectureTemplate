using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices
{
    public class TelerikInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            services.AddTelerikBlazor();
        }
    }
}