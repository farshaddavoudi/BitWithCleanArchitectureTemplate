using MediatR;
using System.Reflection;
using Template.Application.Common.Contracts;
using Template.Application.Common.PipelineBehaviours;
using Template.Domain.Options;

namespace Template.Application.ConfigureServices.Installers;

public class MediatRInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, AppSettings appSettings)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // MediatR Pipelines (Behaviours)
        // Pipelines order matter! Execute from top to bottom 
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}