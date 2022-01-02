using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using RestApi.src.Models;
using Services.Interface;
using Services.Interface.Redis;
using Services;
using Services.Redis;
using StackExchange.Redis;
using System;
using RestApi.Models.Response;
using System.Collections.Generic;
using System.Linq;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        private readonly IInventoryService _inventoryService;

        private readonly IInventorySpecificationService _inventorySpecificationService;

        private readonly ILogger<CartController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(DbContextEntity context, ILogger<CartController> logger,
        IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redisDb)
        {
            _service = new CartService(redisDb);

            _inventoryService = new InventoryService(context);

            _inventorySpecificationService = new InventorySpecificationService(context);

            _logger = logger;

            _httpContextAccessor = httpContextAccessor;
        }

        [Route("")]
        [HttpPost]
        [Authorize()]
        public async Task<IActionResult> Store(CreateCartRequest requestCart)
        {
            if (!await CheckProductIsExist(requestCart))
            {
                return NotFound(new AutResultResponse()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await StoreCart(requestCart);

            return Created("", new AutResultResponse()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task<bool> CheckProductIsExist(CreateCartRequest requestCart)
        {
            return (await _inventoryService.GetById(requestCart.InventoryId) != null) ? true : false;
        }

        private async Task StoreCart(CreateCartRequest requestCart)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];

            await _service.Set(new Cart()
            {
                UserId = user.Id.ToString(),
                InventoryId = requestCart.InventoryId.ToString(),
                Quantity = SumQuantity(user, requestCart).ToString(),
                Attribute = requestCart.Attribute
            });
        }

        [Route("")]
        [HttpDelete]
        [Authorize()]
        public IActionResult Delete(int productId, CartAttributeEnum cartAttribute)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            return _service.Delete(user.Id.ToString(), productId.ToString(), cartAttribute)
            ? NoContent() : NotFound();
        }

        [Route("many/{cartAttribute}")]
        [HttpGet]
        [Authorize()]
        public IActionResult GetMany(CartAttributeEnum cartAttribute)
        {
            var cartList = new List<CartListResponse>();
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            var carts = _service.GetMany(user.Id.ToString(), cartAttribute);
            foreach (var cart in carts)
            {
                var inventory = _inventoryService.GetJoinProduct((int)cart.Name).FirstOrDefault();
                if (inventory != null)
                {
                    var specificationContents = _inventorySpecificationService.GetSpecificationContent(inventory.InventoryId).ToList();
                    cartList.Add(new CartListResponse()
                    {
                        InventoryId = inventory.InventoryId,
                        Name = inventory.ProductName + " " + string.Join("-", specificationContents),
                        Quantity = (int)cart.Value,
                        Price = inventory.Price
                    });
                }
            }
            return Ok(cartList);
        }

        private int SumQuantity(User user, CreateCartRequest requestCart)
        {
            var cart = _service.GetById(user.Id.ToString(), requestCart.InventoryId.ToString(), requestCart.Attribute);
            var quantity = requestCart.Quantity;
            if (cart.Result.HasValue)
            {
                quantity += Convert.ToInt32(cart.Result.ToString());
            }
            return quantity;
        }
    }
}