using ATABit.Model.Data.Contracts;
using Template.Domain.Entities.Order;

namespace Template.Application.Order.Contracts;

public interface IOrderRepository : IATARepository<OrderEntity>
{
}