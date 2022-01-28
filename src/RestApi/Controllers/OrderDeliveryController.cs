using System;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using RestApi.src.Models;
using Services;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDeliveryController : ControllerBase
    {
        private readonly ILogger<OrderDeliveryController> _logger;

        private readonly IOrderService _orderService;

        private readonly IOrderInventoryService _OrderInventoryService;

        private readonly IOrderDeliveryService _orderDeliveryService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IDeliveryService _deliveryService;

        public OrderDeliveryController(DbContextEntity context, ILogger<OrderDeliveryController> logger
        , IHttpContextAccessor httpContextAccessor)
        {
            _orderDeliveryService = new OrderDeliveryService(context);
            _orderService = new OrderService(context);
            _OrderInventoryService = new OrderInventoryService(context);
            _deliveryService = new DeliveryService(context);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize()]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderDeliveryRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (await _orderService.GetUserOrder(request.OrderId, Convert.ToInt32(userJWT.Id)) == null)
            {
                return NotFound();
            }
            var orderDelivery = await InsertOrderDelivery(request);
            return Created("", new AutResultResponse() { Status = true, Data = "" });
        }

        [Authorize()]
        [HttpGet]
        [Route("show/{id}")]
        public async Task<IActionResult> GetByOrderId(int id)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (await _orderService.GetUserOrder(id, Convert.ToInt32(userJWT.Id)) == null)
            {
                return NotFound();
            }
            var orderByDelivery = await _orderDeliveryService.GetByOrderId(id);
            return Ok(await _deliveryService.GetDelivery(orderByDelivery.Type));
        }

        private async Task<OrderDelivery> InsertOrderDelivery(CreateOrderDeliveryRequest request)
        {
            var orderDelivery = new OrderDelivery();
            orderDelivery.OrderId = request.OrderId;
            orderDelivery.Type = request.Type;
            await _orderDeliveryService.Insert(orderDelivery);

            return orderDelivery;
        }
    }
}