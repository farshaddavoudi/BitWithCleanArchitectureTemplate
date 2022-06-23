using Template.Application.Order.Contracts;
using Template.Application.Order.DTOs;
using Template.Domain.Entities.Order;
using Template.Domain.Enums.Order;

namespace Template.Application.Order.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<IQueryable<OrderDto>> GetOrdersAsync(GetOrdersQuery filters, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ReferenceCity> ValidateCityAndCheckCityReportPermission(int cityId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<OrderEntity> GetUserLastOrder(int userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public OrderDto GetFirstOrder()
    {
        return null;

        //return _orderRepository.GetAll()
        //    .ProjectTo<OrderDto>(Mapper.ConfigurationProvider)
        //    .FirstOrDefault();
    }
}