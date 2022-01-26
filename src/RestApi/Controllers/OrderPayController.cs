using System;
using System.Linq;
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
    public class OrderPayController : ControllerBase
    {
        private readonly ILogger<OrderPayController> _logger;

        private readonly IOrderService _orderService;

        private readonly IOrderInventoryService _OrderInventoryService;

        private readonly IOrderPayService _orderPayService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderPayController(DbContextEntity context, ILogger<OrderPayController> logger
        , IHttpContextAccessor httpContextAccessor)
        {
            _orderPayService = new OrderPayService(context);
            _orderService = new OrderService(context);
            _OrderInventoryService = new OrderInventoryService(context);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize()]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderPayRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (await _orderService.GetUserOrder(request.orderId, Convert.ToInt32(userJWT.Id)) == null)
            {
                return NotFound();
            }
            var orderPay = await InsertOrderPay(request);
            return Created("", new AutResultResponse() { Status = true, Data = "" });
        }

        private async Task<OrderPay> InsertOrderPay(CreateOrderPayRequest request)
        {
            var orderPay = new OrderPay();
            orderPay.OrderId = request.orderId;
            orderPay.IsPaid = false;
            orderPay.Terms = request.Terms;
            orderPay.TotalPrice = _OrderInventoryService.GetUserOrderInventoryMany(request.orderId)
            .Sum(x => x.Price * x.Quality);
            await _orderPayService.Insert(orderPay);

            return orderPay;
        }
    }
}