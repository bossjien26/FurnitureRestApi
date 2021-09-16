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
using Services.IService;
using Services.IService.Redis;
using Services.Service;
using Services.Service.Redis;
using StackExchange.Redis;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _repository;

        private readonly IProductService _repositoryProduct;

        private readonly ILogger<CartController> _logger;

        //TODO: Unable to resolve service for type
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(DbContextEntity context, ILogger<CartController> logger,
        IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redisDb)
        {
            _repository = new CartService(redisDb);

            _repositoryProduct = new ProductService(context);

            _logger = logger;

            _httpContextAccessor = httpContextAccessor;
        }

        [Route("store")]
        [HttpPost]
        [Authorize(Enum.Role.SuperAdmin, Enum.Role.Customer, Enum.Role.Admin, Enum.Role.Staff)]
        public async Task<IActionResult> Store(RequestCart requestCart)
        {
            if (!await CheckProductIsExist(requestCart))
            {
                return NotFound(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await StoreCart(requestCart);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task<bool> CheckProductIsExist(RequestCart requestCart)
        {
            return (await _repositoryProduct.GetById(requestCart.ProductId) != null) ? true : false;
        }

        private async Task StoreCart(RequestCart requestCart)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];

            await _repository.Set(new Cart()
            {
                UserId = user.Id.ToString(),
                ProductId = requestCart.ProductId.ToString(),
                Quantity = requestCart.Quantity.ToString(),
                Attribute = requestCart.Attribute
            });
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Enum.Role.SuperAdmin, Enum.Role.Customer, Enum.Role.Admin, Enum.Role.Staff)]
        public IActionResult Delete(int productId, CartAttribute cartAttribute)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            _repository.Delete(user.Id.ToString(), productId.ToString(), cartAttribute);
            return NoContent();
        }
    }
}