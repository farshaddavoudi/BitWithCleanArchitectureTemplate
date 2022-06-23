using Bit.OData.ODataControllers;
using MediatR;
using System.Web.Http;
using Template.Application.Order.DTOs;
using Template.Application.Order.Queries.GetUserLastOrder;

namespace Template.WebUI.API.Controllers.Order;

[AllowAnonymous]
public class OrderController : DtoController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Function]
    public async Task<OrderDto> GetFirstOrder(int userId, CancellationToken cancellationToken)
    {
        var a = await _mediator.Send(new GetUserLastOrderQuery(userId), cancellationToken);

        return a;
    }
}