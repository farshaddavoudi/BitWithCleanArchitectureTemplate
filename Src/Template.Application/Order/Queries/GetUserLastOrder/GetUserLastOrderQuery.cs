using MediatR;
using Template.Application.Order.DTOs;

namespace Template.Application.Order.Queries.GetUserLastOrder;

public record GetUserLastOrderQuery(int UserId) : IRequest<OrderDto>;