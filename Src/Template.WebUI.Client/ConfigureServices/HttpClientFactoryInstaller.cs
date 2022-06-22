using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Template.WebUI.Client.Implementations;
using Template.WebUI.Client.Models;

namespace Template.WebUI.Client.ConfigureServices
{
    public class HttpClientFactoryInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            #region Host client

            services.AddScoped(serviceProvider => serviceProvider.GetService<HostClient>()!.Client);

            services.AddScoped<AppHttpExceptionHandler>();

            #endregion
        }
    }
}