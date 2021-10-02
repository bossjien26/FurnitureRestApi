using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using Services;
using Services.Interface;
using System.Linq;
using System.Collections.Generic;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderProductController : ControllerBase
    {
        private readonly ILogger<OrderProductController> _logger;

        private readonly IOrderService _orderService;

        private readonly IOrderProductService _orderProductService;

        private readonly IProductService _productService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderProductController(DbContextEntity context, ILogger<OrderProductController> logger
        , IHttpContextAccessor httpContextAccessor)
        {
            _orderService = new OrderService(context);
            _orderProductService = new OrderProductService(context);
            _productService = new ProductService(context);
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
            request.productList.ForEach(async x =>
            {
                var orderProductId = await InsertOrderProduct(request.orderId, x);
                if (orderProductId != 0)
                {
                    ids.Add(orderProductId);
                }
            });
            return Created("", ids);
        }

        private async Task<int> InsertOrderProduct(int orderId, CreateOrderProductListRequest request)
        {
            var product = await _productService.GetShowProdcutById(orderId);
            if (product == null)
            {
                return 0;
            }
            var orderProduct = new OrderProduct();
            orderProduct.OrderId = orderId;
            orderProduct.Price = product.Price;
            orderProduct.ProductId = product.Id;
            orderProduct.ProductName = product.Name;
            orderProduct.Quality = request.Quality;
            orderProduct.Specification = request.Specification;
            await _orderProductService.Insert(orderProduct);

            return orderProduct.Id;
        }
    }
}