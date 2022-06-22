using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Template.WebUI.Client.ConfigureServices
{
    public class LoadingBarInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            services.AddLoadingBar();
        }
    }
}