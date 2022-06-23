using ATABit.Model.Data.Contracts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Order.DTOs;
using Template.Domain.Entities.Order;

namespace Template.Application.Order.Queries.GetUserLastOrder;

public class GetUserLastOrderQueryHandler : IRequestHandler<GetUserLastOrderQuery, OrderDto>
{
    private readonly IATARepository<OrderEntity> _orderRepository;
    private readonly IMapper _mapper;

    public GetUserLastOrderQueryHandler(IATARepository<OrderEntity> orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public Task<OrderDto> Handle(GetUserLastOrderQuery request, CancellationToken cancellationToken)
    {
        return _orderRepository.GetAll()
            .Where(o => o.UserId == request.UserId)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}