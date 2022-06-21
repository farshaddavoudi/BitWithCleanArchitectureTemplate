using Template.Application.Common.Contracts;
using Template.Application.Order.DTOs;
using Template.Domain.Entities.Order;
using Template.Domain.Enums.Order;

namespace Template.Application.Order.Contracts;

public interface IOrderService : IEntityService<OrderEntity>
{
    Task<IQueryable<OrderDto>> GetOrdersAsync(GetOrdersQuery filters, CancellationToken cancellationToken);

    Task<ReferenceCity> ValidateCityAndCheckCityReportPermission(int cityId, CancellationToken cancellationToken);

    Task<OrderEntity> GetUserLastOrder(int userId, CancellationToken cancellationToken);

    OrderDto GetFirstOrder();
}