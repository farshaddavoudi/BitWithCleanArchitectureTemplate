using Bit.IdentityServer.Contracts;
using Bit.IdentityServer.Implementations;
using Template.Domain.Shared;

namespace Template.Infrastructure.Identity
{
    public class IdentityClientsProvider : OAuthClientsProvider
    {
        public override IEnumerable<IdentityServer3.Core.Models.Client> GetClients()
        {
            return new[]
            {
                GetResourceOwnerFlowClient(new BitResourceOwnerFlowClient
                {
                    ClientName = AppConstants.WebApp.ClientName,
                    ClientId = AppConstants.WebApp.ClientId,
                    Secret = AppConstants.WebApp.Secret,
                    TokensLifetime = AppConstants.AuthToken.Lifetime,
                    Enabled = true
                })
            };
        }
    }
}
