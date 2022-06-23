using Bit.Core.Contracts;
using Bit.Owin.Implementations;
using System.Reflection;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;
using Template.Infrastructure.LogStore;

namespace Template.Infrastructure.ConfigureServices;

public class LoggerInstaller : IBitInstaller
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