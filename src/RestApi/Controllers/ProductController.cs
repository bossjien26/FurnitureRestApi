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
using Services;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _repository;

        private readonly ILogger<ProductController> _logger;

        private readonly IProductCategoryService _productCategoryRepository;

        private readonly IProductSpecificationService _productSpecificationRepository;

        public ProductController(DbContextEntity context, ILogger<ProductController> logger)
        {
            _repository = new ProductService(context);

            _productCategoryRepository = new ProductCategoryService(context);

            _productSpecificationRepository = new ProductSpecificationService(context);

            _logger = logger;
        }

        [Authorize(Role.SuperAdmin, Role.Admin)]
        [Route("insert")]
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
            var product = await _repository.GetById(id);
            return Ok(product);
        }

        private async Task<Product> InsertProdcut(RequestProduct requestProduct)
        {
            var product = new Product()
            {
                Name = requestProduct.Name,
                Price = requestProduct.Price,
                Sequence = requestProduct.Sequence,
                Quantity = requestProduct.Quantity,
                RelateAt = requestProduct.RelateAt,
                IsDisplay = requestProduct.IsDisplay
            };
            await _repository.Insert(product);
            return product;
        }

        [Authorize(Role.SuperAdmin, Role.Admin, Role.Staff)]
        [Route("showMany/{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage)
        {
            return Ok(_repository.GetMany(perPage, 10).ToList());
        }

        [Authorize(Role.SuperAdmin, Role.Admin)]
        [Route("insertProductCategory")]
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
            return _repository.CheckProductToProductCategoryIsExist(requestProductCategory.ProductId
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

        [Authorize(Role.SuperAdmin, Role.Admin)]
        [Route("insertProductSpecification")]
        [HttpPost]
        public async Task<IActionResult> StoreProductSpecification(RequestProductSpecification requestProductSpecification)
        {
            if (CheckProductAndSpecificationIsExist(requestProductSpecification))
            {
                return NotFound(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }
            await InsertProductSpecification(requestProductSpecification);

            return CreatedAtAction(nameof(GetProduct), new { id = requestProductSpecification.ProductId },
                new AutResultModel()
                {
                    Status = true,
                    Data = "Success"
                });
        }

        private bool CheckProductAndSpecificationIsExist(RequestProductSpecification requestProductSpecification)
        {
            return _repository.CheckProductAndProductSpecificationIsExist(requestProductSpecification.ProductId,
            requestProductSpecification.SpecificationId) ? true : false;
        }

        private async Task InsertProductSpecification(RequestProductSpecification requestProductSpecification)
        {
            await _productSpecificationRepository.Insert(new ProductSpecification()
            {
                ProductId = requestProductSpecification.ProductId,
                SpecificationId = requestProductSpecification.SpecificationId
            });
        }
    }
}