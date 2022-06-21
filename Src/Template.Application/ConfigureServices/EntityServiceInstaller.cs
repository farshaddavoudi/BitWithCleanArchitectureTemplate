using Bit.Core.Contracts;
using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Application.Common.Implementations;
using Template.Domain.Options;

namespace Template.Application.ConfigureServices;

public class EntityServiceInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        dependencyManager.RegisterGeneric(typeof(IReadOnlyEntityService<>).GetTypeInfo(), typeof(ReadOnlyEntityService<>).GetTypeInfo());
        dependencyManager.RegisterGeneric(typeof(IEntityService<>).GetTypeInfo(), typeof(EntityService<>).GetTypeInfo());
    }
}