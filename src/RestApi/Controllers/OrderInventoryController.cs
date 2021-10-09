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
using System.Text;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderInventoryController : ControllerBase
    {
        private readonly ILogger<OrderInventoryController> _logger;

        private readonly IOrderService _orderService;

        private readonly ICartService _cartService;

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
        }

        [Authorize()]
        [HttpGet]
        [Route("{perPage}")]
        public IActionResult ShowMany(int perPage)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            return Ok(_inventoryService.GetShowMany(perPage, 5).ToList<Inventory>());
        }

        [Authorize()]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderInventoryRequest request)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            if (await _orderService.GetUserOrder(request.orderId, user.Id) == null)
            {
                return NotFound();
            }
            var ids = new List<int>();
            var carts = _cartService.GetMany(user.Id.ToString(), CartAttributeEnum.Shopping);
            foreach (var cart in carts)
            {
                var OrderInventoryId = await InsertOrderInventory(request.orderId, cart);
                if (OrderInventoryId != 0)
                {
                    ids.Add(OrderInventoryId);
                    _cartService.Delete(user.Id.ToString(), cart.Name, CartAttributeEnum.Shopping);
                }
            }
            return Created("", ids);
        }

        private async Task<int> InsertOrderInventory(int orderId, HashEntry cart)
        {
            var inventory = _inventoryService.GetJoinProductAndSpecification((int)cart.Name).FirstOrDefault();
            if (inventory == null)
            {
                return 0;
            }
            var orderInventory = new OrderInventory()
            {
                OrderId = orderId,
                Price = inventory.Price,
                InventoryId = inventory.Id,
                ProductName = inventory.Product.Name,
                Quality = (int)cart.Value,
                Specification = inventory.InventorySpecifications.Select(x =>
                x.Specification.Name).Aggregate(
                    new StringBuilder(),
                    (current, next) => current.Append(current.Length == 0 ? "" : "-").Append(next)
                )
                .ToString()
            };

            await _OrderInventoryService.Insert(orderInventory);
            return orderInventory.Id;
        }
    }
}