using System;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Enum;
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
    public class OrderStatusesController : ControllerBase
    {
        private readonly ILogger<OrderStatusesController> _logger;

        private readonly IOrderService _orderService;

        private readonly IOrderStatusesService _orderStatusesService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderStatusesController(DbContextEntity context, ILogger<OrderStatusesController> logger
        , IHttpContextAccessor httpContextAccessor)
        {
            _orderStatusesService = new OrderStatusesService(context);
            _orderService = new OrderService(context);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(RoleEnum.Staff, RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderStatusesRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (await _orderService.GetUserOrder(request.OrderId, Convert.ToInt32(userJWT.Id)) == null)
            {
                return NotFound();
            }
            await InsertOrderStatus(request.OrderId, request.Status);
            return Created("", new AutResultResponse() { Status = true, Data = "" });
        }

        [Authorize()]
        [HttpGet]
        [Route("show/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (await _orderService.GetUserOrder(orderId, Convert.ToInt32(userJWT.Id)) == null)
            {
                return NotFound();
            }
            return Ok(_orderStatusesService.GetByOrderId(orderId).ToList());
        }

        private async Task InsertOrderStatus(int orderId, OrderStatusEnum status)
        {
            await _orderStatusesService.Insert(new Entities.OrderStatuses()
            {
                OrderId = orderId,
                Status = status
            });
        }
    }
}