using Blazored.Modal;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices
{
    public class ModalInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings)
        {
            // https://github.com/Blazored/Modal
            services.AddBlazoredModal();
        }
    }
}