using ATABit.Model.Data.Contracts;
using MediatR;
using Template.Domain.Entities.Identity;

namespace Template.Application.DTOs;

public class GetIdentityTokenByIdQueryHandler : IRequestHandler<GetIdentityTokenByIdQuery, IdentityTokenEntity>
{
    private readonly IATARepository<IdentityTokenEntity> _identityTokenRepository;

    public GetIdentityTokenByIdQueryHandler(IATARepository<IdentityTokenEntity> identityTokenRepository)
    {
        _identityTokenRepository = identityTokenRepository;
    }

    public Task<IdentityTokenEntity> Handle(GetIdentityTokenByIdQuery request, CancellationToken cancellationToken)
    {
        return _identityTokenRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}