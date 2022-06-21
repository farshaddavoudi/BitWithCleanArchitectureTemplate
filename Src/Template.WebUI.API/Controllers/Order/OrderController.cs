using Bit.OData.ODataControllers;
using System.Web.Http;
using Template.Application.Order.Contracts;
using Template.Application.Order.DTOs;

namespace Template.WebUI.API.Controllers.Order;

[AllowAnonymous]
public class OrderController : DtoController
{
    private readonly IOrderService _orderService;

    #region Constructor Injections

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    #endregion

    [Function]
    public OrderDto GetFirstOrder()
    {
        return _orderService.GetFirstOrder();
    }
}