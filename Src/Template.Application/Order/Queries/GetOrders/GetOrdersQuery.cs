using MediatR;
using Template.Application.Order.DTOs;

namespace Template.Application.Order.Queries.GetOrders;

public class GetOrdersQuery : IRequest<IQueryable<OrderDto>>
{

}