using Bit.Core.Contracts;
using Bit.Http.Contracts;
using Bit.Http.Implementations;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices
{
    public class BitInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
        {
            services.AddScoped<ISecurityService, DefaultSecurityService>();
            services.AddTransient<ITokenProvider, DefaultTokenProvider>();
        }
    }
}