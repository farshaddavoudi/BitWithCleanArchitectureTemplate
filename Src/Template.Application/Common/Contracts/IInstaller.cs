using Bit.Core.Contracts;
using Template.Domain.Options;

namespace Template.Application.Common.Contracts;

public interface IInstaller
{
    void InstallServices(IServiceCollection services,
        IDependencyManager dependencyManager,
        AppSettings appSettings);
}