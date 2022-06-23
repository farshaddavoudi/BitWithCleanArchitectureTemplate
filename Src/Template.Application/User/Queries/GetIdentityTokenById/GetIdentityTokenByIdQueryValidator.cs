using ATABit.Model.Data.Contracts;
using FluentValidation;
using Template.Domain.Entities.Identity;

namespace Template.Application.DTOs;

public class GetIdentityTokenByIdQueryValidator : AbstractValidator<GetIdentityTokenByIdQuery>
{
    private readonly IATARepository<IdentityTokenEntity> _identityTokenRepository;

    public GetIdentityTokenByIdQueryValidator(IATARepository<IdentityTokenEntity> identityTokenRepository)
    {
        _identityTokenRepository = identityTokenRepository;

        //RuleFor(v => v.Title)
        //    .NotEmpty().WithMessage("Title is required.")
        //    .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
        //    .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await Task.FromResult(true);
        //return await _context.TodoLists
        //    .AllAsync(l => l.Title != title, cancellationToken);
    }
}