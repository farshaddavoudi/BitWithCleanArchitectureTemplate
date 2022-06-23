using AutoMapper;
using Template.Application.Order.DTOs;
using Template.Domain.Entities.Order;

namespace Template.Application.Common.Mappings;

public class OrderMapperProfile : Profile
{
    public OrderMapperProfile()
    {
        CreateMap<OrderEntity, OrderDto>()
            .ForMember(dto => dto.OrderStatus, config =>
                config.MapFrom(entity => (int)entity.OrderStatus));
    }
}