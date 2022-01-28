using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using Services;
using Services.Interface;
using System.Collections.Generic;
using Services.Interface.Redis;
using Services.Redis;
using StackExchange.Redis;
using Enum;
using RestApi.Models.Requests;
using System.Linq;
using System;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderInventoryController : ControllerBase
    {
        private readonly ILogger<OrderInventoryController> _logger;

        private readonly IOrderService _orderService;

        private readonly ICartService _cartService;

        private readonly IInventorySpecificationService _inventorySpecificationService;

        private readonly IOrderInventoryService _OrderInventoryService;

        private readonly IInventoryService _inventoryService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderInventoryController(DbContextEntity context, IConnectionMultiplexer redis
        , ILogger<OrderInventoryController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = new OrderService(context);
            _OrderInventoryService = new OrderInventoryService(context);
            _cartService = new CartService(redis);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _inventoryService = new InventoryService(context);
            _inventorySpecificationService = new InventorySpecificationService(context);
        }

        [Authorize()]
        [HttpGet]
        [Route("show/{orderId}")]
        public IActionResult ShowMany(int orderId)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            //TODO:get user order
            return Ok(_OrderInventoryService.GetUserOrderInventoryMany(orderId).ToList());
        }

        [Authorize()]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderInventoryRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (await _orderService.GetUserOrder(request.orderId, Convert.ToInt32(userJWT.Id)) == null)
            {
                return NotFound();
            }
            var ids = new List<int>();
            var carts = _cartService.GetMany(userJWT.Id, CartAttributeEnum.Shopping);
            foreach (var cart in carts)
            {
                var OrderInventoryId = await InsertOrderInventory(request.orderId, cart);
                if (OrderInventoryId != 0)
                {
                    ids.Add(OrderInventoryId);
                    _cartService.Delete(userJWT.Id, cart.Name, CartAttributeEnum.Shopping);
                }
            }
            return Created("", ids);
        }

        private async Task<int> InsertOrderInventory(int orderId, HashEntry cart)
        {
            var inventoryToOrderInventory = _inventoryService.GetJoinProduct((int)cart.Name).FirstOrDefault();
            if (inventoryToOrderInventory == null)
            {
                return 0;
            }

            var specificationContents = _inventorySpecificationService.GetSpecificationContent(inventoryToOrderInventory.InventoryId).ToList();

            var orderInventory = new OrderInventory()
            {
                OrderId = orderId,
                Price = inventoryToOrderInventory.Price,
                InventoryId = inventoryToOrderInventory.InventoryId,
                ProductName = inventoryToOrderInventory.ProductName + " " + string.Join("-", specificationContents),
                Quality = (int)cart.Value,
            };

            await _OrderInventoryService.Insert(orderInventory);
            return orderInventory.Id;
        }
    }
}