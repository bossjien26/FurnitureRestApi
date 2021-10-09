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
using Services.Interface;
using Services;
using System;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        private readonly ILogger<ProductController> _logger;

        private readonly IProductCategoryService _productCategoryRepository;

        public ProductController(DbContextEntity context, ILogger<ProductController> logger)
        {
            _service = new ProductService(context);

            _productCategoryRepository = new ProductCategoryService(context);

            _logger = logger;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Insert(RequestProduct requestsProduct)
        {
            var product = await InsertProdcut(requestsProduct);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _service.GetById(id);
            return Ok(product);
        }

        private async Task<Product> InsertProdcut(RequestProduct requestProduct)
        {
            var product = new Product()
            {
                Name = requestProduct.Name,
                CreateAt = DateTime.Now
            };
            await _service.Insert(product);
            return product;
        }

        [Route("{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage)
        {
            return Ok(_service.GetMany(perPage, 10).ToList());
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [Route("store/productCategory")]
        [HttpPost]
        public async Task<IActionResult> StoreProductCategory(RequestProductCategory requestProductCategory)
        {
            if (CheckProductAndCategoryIsExist(requestProductCategory))
            {
                return NotFound(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }
            await InsertProductCategory(requestProductCategory);

            return CreatedAtAction(nameof(GetProduct), new { id = requestProductCategory.ProductId },
                new AutResultModel()
                {
                    Status = true,
                    Data = "Success"
                });
        }

        private bool CheckProductAndCategoryIsExist(RequestProductCategory requestProductCategory)
        {
            return _service.CheckProductToProductCategoryIsExist(requestProductCategory.ProductId
            , requestProductCategory.CategoryId) ? true : false;
        }

        private async Task InsertProductCategory(RequestProductCategory requestProductCategory)
        {
            await _productCategoryRepository.Insert(new ProductCategory()
            {
                ProductId = requestProductCategory.ProductId,
                CategoryId = requestProductCategory.CategoryId
            });
        }
    }
}