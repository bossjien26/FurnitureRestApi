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
using Services.Service;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _repository;

        private readonly IProductService _repositoryProduct;

        private readonly ILogger<CartController> _logger;

        private readonly HttpContext _httpContext;

        public CartController(DbContextEntity context, ILogger<CartController> logger,
        HttpContext httpContext)
        {
            _repository = new CartService(context);

            _repositoryProduct = new ProductService(context);

            _logger = logger;

            _httpContext = httpContext;
        }

        [Route("insert")]
        [HttpPost]
        [Authorize(Role.SuperAdmin, Role.Customer, Role.Admin, Role.Staff)]
        public async Task<IActionResult> Insert(RequestCart requestCart)
        {
            if (!await CheckProductIsExist(requestCart))
            {
                return Ok(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await InsertCart(requestCart);

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

        private async Task InsertCart(RequestCart requestCart)
        {
            var user = (User)_httpContext.Items["User"];

            await _repository.Insert(new Cart()
            {
                UserId = user.Id,
                ProductId = requestCart.ProductId,
                Quantity = requestCart.Quantity,
                Attribute = requestCart.Attribute
            });
        }

        [Route("insert")]
        [HttpPost]
        [Authorize(Role.SuperAdmin, Role.Customer, Role.Admin, Role.Staff)]
        public async Task<IActionResult> Update(RequestCart requestCart)
        {
            var user = (User)_httpContext.Items["User"];

            var checkParametrComfirm = await CheckParametrComfirm(requestCart,user.Id);
            
            if (!checkParametrComfirm.Status){return Ok(checkParametrComfirm);}

            UpdateCart(requestCart, user.Id);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task<AutResultModel> CheckParametrComfirm(RequestCart requestCart, int userId)
        {
            if (!await CheckProductAndCartIsExist(requestCart, userId))
            {
                return new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                };
            }

            if (requestCart.Quantity < 1)
            {
                return new AutResultModel()
                {
                    Status = false,
                    Data = "The quantity need greater than or equal to one"
                };
            }

            return new AutResultModel(){Status=true};
        }

        private async Task<bool> CheckProductAndCartIsExist(RequestCart requestCart, int userId)
        {
            return (await _repositoryProduct.GetById(requestCart.ProductId) != null &&
            await _repository.GetUserCart(requestCart.Id, userId, requestCart.ProductId)
             != null) ? true : false;
        }

        private void UpdateCart(RequestCart requestCart, int userId)
        {
            _repository.Update(new Cart()
            {
                UserId = userId,
                ProductId = requestCart.ProductId,
                Quantity = requestCart.Quantity,
                Attribute = requestCart.Attribute
            });
        }
    }
}