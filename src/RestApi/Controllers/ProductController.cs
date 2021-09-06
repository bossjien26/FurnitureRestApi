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
        public IActionResult ShowMany(RequestPage requestPage)
        {
            return Ok(_repository.GetMany(requestPage.Pages, 10).ToList());
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
            return (await _productCategoryRepository.GetById(requestProductCategory.CategoryId) != null 
            && await _repository.GetById(requestProductCategory.ProductId) != null)? true:false;
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
            if(! await CheckProductAndSpecificationIsExist(requestProductSpecification))
            {
                return Ok(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }
            await InsertProductSpecification(requestProductSpecification);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task<bool> CheckProductAndSpecificationIsExist(RequestProductSpecification requestProductSpecification)
        {
            return (await _productSpecificationRepository.GetById(requestProductSpecification.SpecificationId) != null 
            && await _repository.GetById(requestProductSpecification.ProductId) != null)? true:false;
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