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

        [Route("")]
        [HttpPost]
        [Authorize(Enum.RoleEnum.SuperAdmin, Enum.RoleEnum.Customer, Enum.RoleEnum.Admin, Enum.RoleEnum.Staff)]
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

            return Created("",new AutResultModel()
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

        [Route("")]
        [HttpDelete]
        [Authorize(Enum.RoleEnum.SuperAdmin, Enum.RoleEnum.Customer, Enum.RoleEnum.Admin, Enum.RoleEnum.Staff)]
        public IActionResult Delete(int productId, CartAttributeEnum cartAttribute)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            return _repository.Delete(user.Id.ToString(), productId.ToString(), cartAttribute)
            ? NoContent() : NotFound();
        }
    }
}