using Bit.Core.Contracts;
using Blazored.Modal;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices
{
    public class ModalInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            // https://github.com/Blazored/Modal
            services.AddBlazoredModal();
        }
    }
}