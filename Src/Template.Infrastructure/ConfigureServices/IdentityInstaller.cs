using ATABit.Model.User.Contract;
using Bit.Core.Contracts;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;
using Template.Infrastructure.Identity;

namespace Template.Infrastructure.ConfigureServices;

public class IdentityInstaller : IBitInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        dependencyManager.Register<IUserInfoProvider, UserInfoProvider>();
    }
}