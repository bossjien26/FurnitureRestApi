using System.Linq;
using DbEntity;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApi.Models.Requests;
using Services;
using Services.Dto;
using Services.Interface;

namespace RestApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductSpecificationController : ControllerBase
    {
        private readonly ILogger<ProductSpecificationController> _logger;

        private readonly MailHelper _mailHelper;

        private readonly IProductSpecificationService _service;

        public ProductSpecificationController(DbContextEntity context, ILogger<ProductSpecificationController> logger
        , MailHelper mailHelper)
        {
            _service = new ProductSpecificationService(context);
            _logger = logger;
            _mailHelper = mailHelper;
        }

        [Route("{productId}")]
        [HttpGet]
        public IActionResult Show(int productId) => Ok(_service.GetMany(productId).ToList());

        [Route("detail")]
        [HttpPost]
        public IActionResult Detail(GetProductSpecificationRequest getProductSpecification)
        => Ok(_service.GetOneJoinSpecificationByProductId(getProductSpecification.ProductId
                , getProductSpecification.Offset, getProductSpecification.PreviousSpecifications).ToList()
                .Select(x => new InventoryWithSpecification()
                {
                    Id = x.SpecificationId,
                    Name = x.Specification.Name,
                    SpecificationContentLists = x.InventorySpecifications.Select(y =>
                    new InventoryWithSpecification()
                    {
                        Id = y.SpecificationContent.Id,
                        Name = y.SpecificationContent.Name
                    }).ToList()
                })
            );
    }
}