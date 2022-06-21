using Template.Application.Common.Contracts;
using Template.Domain.Entities.Identity;

namespace Template.Application.Identity.Contracts;

public interface IIdentityTokenService : IEntityService<IdentityTokenEntity>
{
    public Task<IdentityTokenEntity?> GetIdentityTokenById(int id, CancellationToken cancellationToken);
}