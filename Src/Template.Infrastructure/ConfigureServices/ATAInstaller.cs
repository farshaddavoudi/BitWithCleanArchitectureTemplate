using ATA.Broker.CDN;
using ATA.Broker.SSOSecurity;
using ATA.Broker.Workflow;
using ATABit.Data;
using Bit.Core.Contracts;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.Infrastructure.ConfigureServices;

public class ATAInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        services.AddATABitDataAccess();

        services.AddATACDN();

        services.AddATASSOClient();

        services.AddATAWorkflowClient();
    }
}