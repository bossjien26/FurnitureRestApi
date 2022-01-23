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

        private readonly IUserService _userService;

        private readonly IInventoryService _inventoryService;

        private readonly IInventorySpecificationService _inventorySpecificationService;

        private readonly ILogger<CartController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(DbContextEntity context, ILogger<CartController> logger,
        IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redisDb)
        {
            _service = new CartService(redisDb);

            _userService = new UserService(context, redisDb);

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
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            if (!await CheckProductIsExist(requestCart))
            {
                return NotFound(new AutResultResponse()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await StoreCart(requestCart, userJWT.Id);

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

        private async Task StoreCart(CreateCartRequest requestCart, string userId)
        {
            await _service.Set(new Cart()
            {
                UserId = userId,
                InventoryId = requestCart.InventoryId.ToString(),
                Quantity = SumQuantity(userId, requestCart).ToString(),
                Attribute = requestCart.Attribute
            });
        }

        [Route("{cartAttribute}/{inventoryId}")]
        [HttpDelete]
        [Authorize()]
        public async Task<IActionResult> Delete(int inventoryId, CartAttributeEnum cartAttribute)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var user = await _userService.GetById(Convert.ToInt32(userJWT.Id));
            return _service.Delete(userJWT.Id, inventoryId.ToString(), cartAttribute)
            ? NoContent() : NotFound();
        }

        [Route("many/{cartAttribute}")]
        [HttpGet]
        [Authorize()]
        public IActionResult GetMany(CartAttributeEnum cartAttribute)
        {
            var cartList = new List<CartListResponse>();
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var carts = _service.GetMany(userJWT.Id, cartAttribute);
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

        private int SumQuantity(string userId, CreateCartRequest requestCart)
        {
            var cart = _service.GetById(userId, requestCart.InventoryId.ToString(), requestCart.Attribute);
            var quantity = requestCart.Quantity;
            if (cart.Result.HasValue)
            {
                quantity += Convert.ToInt32(cart.Result.ToString());
            }
            return quantity;
        }
    }
}