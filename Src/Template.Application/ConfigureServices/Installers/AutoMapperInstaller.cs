using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Domain.Options;

namespace Template.Application.ConfigureServices.Installers;

public class AutoMapperInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, AppSettings appSettings)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}