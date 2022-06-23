using MediatR;
using System.ComponentModel.DataAnnotations.Schema;
using Template.Domain.Entities.Identity;

namespace Template.Application.DTOs;

[ComplexType]
public record GetIdentityTokenByIdQuery(int Id) : IRequest<IdentityTokenEntity>;