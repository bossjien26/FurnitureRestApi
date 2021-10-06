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

        private readonly IProductService _productService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderInventoryController(DbContextEntity context, IConnectionMultiplexer redis
        , ILogger<OrderInventoryController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = new OrderService(context);
            _OrderInventoryService = new OrderInventoryService(context);
            _productService = new ProductService(context);
            _cartService = new CartService(redis);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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
            // request.productList.ForEach(async x =>
            // {
            //     var OrderInventoryId = await InsertOrderInventory(request.orderId, x);
            //     if (OrderInventoryId != 0)
            //     {
            //         ids.Add(OrderInventoryId);
            //     }
            // });
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
            return Created("", request);
        }

        //TODO:use redis get user cart list
        private async Task<int> InsertOrderInventory(int orderId, HashEntry cart)
        {
            var product = await _productService.GetShowProdcutById(orderId);
            if (product == null)
            {
                return 0;
            }
            var OrderInventory = new OrderInventory();
            OrderInventory.OrderId = orderId;
            // OrderInventory.Price = product.Price;
            OrderInventory.ProductId = (int)cart.Name;
            OrderInventory.ProductName = product.Name;
            OrderInventory.Quality = (int)cart.Value;
            // OrderInventory.Specification = product.InventorySpecifications.
            await _OrderInventoryService.Insert(OrderInventory);
            return OrderInventory.Id;
        }
    }
}