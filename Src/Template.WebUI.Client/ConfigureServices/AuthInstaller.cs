using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Template.Application.Common.Contracts;
using Template.Domain.Options;
using Template.Domain.Shared;
using Template.WebUI.Client.Implementations;

namespace Template.WebUI.Client.ConfigureServices;

public class AuthInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, AppSettings appSettings)
    {
        services.AddOptions();

        services.AddAuthorizationCore(config =>
            {
                var claims = Claims.GetAllAppClaims().ToList();

                foreach (var claim in claims)
                {
                    config.AddPolicy(claim,
                        new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .RequireClaim(claim)
                            .Build());
                }
            }
        );

        services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

        services.AddTransient(serviceProvider => (AppAuthenticationStateProvider)serviceProvider.GetRequiredService<AuthenticationStateProvider>());
    }
}