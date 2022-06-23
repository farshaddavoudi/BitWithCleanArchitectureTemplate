using Template.Application.Order.Contracts;
using Template.Domain.Entities.Order;

namespace Template.Infrastructure.Persistence.EF.Repository.Order;

public class OrderRepository : ATARepository<OrderEntity>, IOrderRepository
{

}