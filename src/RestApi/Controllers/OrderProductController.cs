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
using System;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderProductController : ControllerBase
    {
        private readonly ILogger<OrderProductController> _logger;

        private readonly IOrderService _orderService;

        private readonly ICartService _cartService;

        private readonly IOrderProductService _orderProductService;

        private readonly IProductService _productService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderProductController(DbContextEntity context, IConnectionMultiplexer redis
        , ILogger<OrderProductController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _orderService = new OrderService(context);
            _orderProductService = new OrderProductService(context);
            _productService = new ProductService(context);
            _cartService = new CartService(redis);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize()]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderProductRequest request)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            if (await _orderService.GetUserOrder(request.orderId, user.Id) == null)
            {
                return NotFound();
            }
            var ids = new List<int>();
            // request.productList.ForEach(async x =>
            // {
            //     var orderProductId = await InsertOrderProduct(request.orderId, x);
            //     if (orderProductId != 0)
            //     {
            //         ids.Add(orderProductId);
            //     }
            // });
            var carts = _cartService.GetMany(user.Id.ToString(), CartAttributeEnum.Shopping);
            foreach (var cart in carts)
            {
                var orderProductId = await InsertOrderProduct(request.orderId, cart);
                if (orderProductId != 0)
                {
                    ids.Add(orderProductId);
                    _cartService.Delete(user.Id.ToString(), cart.Name, CartAttributeEnum.Shopping);
                }
            }
            return Created("", request);
        }

        //TODO:use redis get user cart list
        private async Task<int> InsertOrderProduct(int orderId, HashEntry cart)
        {
            var product = await _productService.GetShowProdcutById(orderId);
            if (product == null)
            {
                return 0;
            }
            var orderProduct = new OrderProduct();
            orderProduct.OrderId = orderId;
            orderProduct.Price = product.Price;
            orderProduct.ProductId = (int)cart.Name;
            orderProduct.ProductName = product.Name;
            orderProduct.Quality = (int)cart.Value;
            // orderProduct.Specification = product.ProductSpecifications.
            await _orderProductService.Insert(orderProduct);
            return orderProduct.Id;
        }
    }
}