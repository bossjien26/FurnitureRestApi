using System;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _repository;

        private readonly ILogger<ProductController> _logger;

        private readonly IProductCategoryService _ProductCategoryRepository;

        public ProductController(DbContextEntity context, ILogger<ProductController> logger)
        {
            _repository = new ProductService(context);

            _ProductCategoryRepository = new ProductCategoryService(context);

            _logger = logger;
        }

        [Authorize(Role.SuperAdmin, Role.Admin)]
        [Route("insert")]
        [HttpPost]
        public async Task<IActionResult> Insert(RequestProduct requestsProduct)
        {
            await InsertProdcut(requestsProduct);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task InsertProdcut(RequestProduct requestProduct)
        {
            await _repository.Insert(new Product()
            {
                Name = requestProduct.Name,
                Price = requestProduct.Price,
                Sequence = requestProduct.Sequence,
                Quantity = requestProduct.Quantity,
                RelateAt = requestProduct.RelateAt,
                IsDisplay = requestProduct.IsDisplay
            });
        }

        [Authorize(Role.SuperAdmin, Role.Admin, Role.Staff)]
        [Route("many")]
        [HttpGet]
        public IActionResult ShowMany(int pages)
        {
            return Ok(_repository.GetMany(pages, 10).ToList());
        }

        [Authorize(Role.SuperAdmin, Role.Admin)]
        [Route("insertProductCategory")]
        [HttpPost]
        public async Task<IActionResult> StoreProductCategory(RequestProductCategory requestProductCategory)
        {
            if(! await CheckProductAndCategoryIsExist(requestProductCategory))
            {
                return Ok(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }
            await InsertProductCategory(requestProductCategory);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task<bool> CheckProductAndCategoryIsExist(RequestProductCategory requestProductCategory)
        {
            return (await _ProductCategoryRepository.GetById(requestProductCategory.CategoryId) != null 
            && await _repository.GetById(requestProductCategory.ProductId) != null)? true:false;
        }

        private async Task InsertProductCategory(RequestProductCategory requestProductCategory)
        {
            await _ProductCategoryRepository.Insert(new ProductCategory()
            {
                ProductId = requestProductCategory.ProductId,
                CategoryId = requestProductCategory.CategoryId
            });
        }
    }
}