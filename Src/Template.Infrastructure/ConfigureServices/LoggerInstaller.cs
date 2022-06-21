using Bit.Core.Contracts;
using Bit.Owin.Implementations;
using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Template.Infrastructure.LogStore;

namespace Template.Infrastructure.ConfigureServices;

public class LoggerInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        dependencyManager.RegisterDefaultLogger(typeof(SeqLogStore).GetTypeInfo()
#if DEBUG
            , typeof(DebugLogStore).GetTypeInfo()
            , typeof(ConsoleLogStore).GetTypeInfo()
#endif
        );
    }
}