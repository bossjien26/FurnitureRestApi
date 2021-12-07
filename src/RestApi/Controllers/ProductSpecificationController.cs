using System.Collections.Generic;
using System.Linq;
using DbEntity;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestApi.Models.Requests;
using RestApi.Models.Response;
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

        private readonly IInventorySpecificationService _inventorySpecificationService;

        public ProductSpecificationController(DbContextEntity context, ILogger<ProductSpecificationController> logger
        , MailHelper mailHelper)
        {
            _service = new ProductSpecificationService(context);
            _inventorySpecificationService = new InventorySpecificationService(context);
            _logger = logger;
            _mailHelper = mailHelper;
        }

        [Route("{productId}")]
        [HttpGet]
        public IActionResult Show(int productId) => Ok(_service.GetMany(productId).ToList());

        [Route("detail/{productId}")]
        [HttpGet]
        public IActionResult showByProductIdDetail(int productId)
        => Ok(_service.GetManyJoinSpecification(productId).ToList());

        [Route("nextSpecification")]
        [HttpPost]
        public IActionResult Detail(GetProductSpecificationRequest getProductSpecification)
        {
            var specificationIds = getProductSpecification.Specifications.ToList();
            var basicSpecifications = new List<InventoryIdBySpecifications>();
            _service.GetManyJoinSpecification(getProductSpecification.ProductId).ToList().ForEach(r => {
                basicSpecifications.Add(new InventoryIdBySpecifications(){
                    Id = r.Id,
                    Name = r.Name
                });
            });
            var specificationContents = _service.GetBySpecificationContent(getProductSpecification.ProductId, specificationIds).ToList();
            var inventoryIds = _service.GetOneJoinSpecificationByProductId(getProductSpecification.ProductId
                , specificationIds).ToList();
            var nextSpecification = _service.GetByNextSpecification(getProductSpecification.ProductId, specificationIds.LastOrDefault()).FirstOrDefault();
            if(nextSpecification == null)
            {
                return Ok("no specification");
            }
            var nextInventorySpecification = _service.GetByInventoryIds(inventoryIds, nextSpecification.Id).ToList();
            nextInventorySpecification.RemoveAll(x => specificationContents.Where(z => z.InventoryIdBySpecificationContent.Id == x.InventoryIdBySpecificationContent.Id).Any());
            specificationContents.AddRange(nextInventorySpecification);
            specificationContents.AddRange(basicSpecifications);
            var specifications = specificationContents.GroupBy(x => x.Id).ToDictionary(x => x.Key, x => x.ToList());
            return Ok(TransferSpecification(specifications).ToList());
        }

        private Dictionary<int, SpecificationResponse> TransferSpecification(Dictionary<int, List<InventoryIdBySpecifications>> specifications)
        {
            var specificationResponses = new Dictionary<int, SpecificationResponse>();
            foreach (var specification in specifications)
            {
                var specificationContentResponses = new List<SpecificationContentResponse>();
                specification.Value.ForEach(x =>
                {
                    if(x.InventoryIdBySpecificationContent.Name != null){
                        specificationContentResponses.Add(
                        new SpecificationContentResponse()
                        {
                            Id = x.InventoryIdBySpecificationContent.Id,
                            Name = x.InventoryIdBySpecificationContent.Name
                        });
                    }
                });
                specificationResponses.Add(specification.Key, new SpecificationResponse()
                {
                    Id = specification.Key,
                    Name = specification.Value.First().Name,
                    SpecificationContentResponses = specificationContentResponses
                });
            }

            return specificationResponses;
        }
    }
}