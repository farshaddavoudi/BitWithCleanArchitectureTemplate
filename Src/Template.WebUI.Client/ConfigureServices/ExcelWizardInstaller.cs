using Bit.Core.Contracts;
using ExcelWizard;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.WebUI.Client.ConfigureServices;

public class ExcelWizardInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager, AppSettings appSettings)
    {
        services.AddExcelWizardServices(true);
    }
}