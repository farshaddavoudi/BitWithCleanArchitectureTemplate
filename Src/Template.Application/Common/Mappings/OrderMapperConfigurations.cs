using AutoMapper;
using Bit.Model.Contracts;
using Template.Application.Order.DTOs;
using Template.Domain.Entities.Order;

namespace Template.Application.Common.Mappings;

public class OrderMapperConfigurations : IMapperConfiguration
{
    public void Configure(IMapperConfigurationExpression mapperConfigExpression)
    {
        // OrderEntity => OrderQueryResult
        mapperConfigExpression.CreateMap<OrderEntity, OrderDto>()
            .ForMember(dto => dto.OrderStatus, config =>
                config.MapFrom(entity => (int)entity.OrderStatus));
    }
}