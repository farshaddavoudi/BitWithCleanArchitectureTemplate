using ATA.Broker.CDN;
using ATA.Broker.SSOSecurity;
using ATA.Broker.Workflow;
using ATABit.Data;
using Bit.Core.Contracts;
using Template.Domain.Options;
using Template.Infrastructure.Common.Contracts;

namespace Template.Infrastructure.ConfigureServices;

public class ATAInstaller : IBitInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        services.AddATABitDataAccess();

        services.AddATACDN();

        services.AddATASSOClient();

        services.AddATAWorkflowClient();
    }
}