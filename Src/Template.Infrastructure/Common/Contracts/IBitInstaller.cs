using Bit.Core.Contracts;
using Template.Domain.Options;

namespace Template.Infrastructure.Common.Contracts;

public interface IBitInstaller
{
    void InstallServices(IServiceCollection services,
        IDependencyManager dependencyManager,
        AppSettings appSettings);
}