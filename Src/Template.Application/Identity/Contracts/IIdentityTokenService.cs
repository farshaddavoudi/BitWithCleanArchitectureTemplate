using Template.Domain.Entities.Identity;

namespace Template.Application.Identity.Contracts;

public interface IIdentityTokenService
{
    public Task<IdentityTokenEntity?> GetIdentityTokenById(int id, CancellationToken cancellationToken);
}