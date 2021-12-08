using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using RestApi.src.Models;
using Services;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventorySpecificationController : ControllerBase
    {
        private readonly ILogger<InventorySpecificationController> _logger;

        private readonly IInventorySpecificationService _service;

        public InventorySpecificationController(DbContextEntity context, ILogger<InventorySpecificationController> logger)
        {
            _service = new InventorySpecificationService(context);
            _logger = logger;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Insert(InventorySpecificationRequest request)
        {
            if (await CheckProductAndSpecificationIsExist(request))
            {
                return NotFound();
            }
            await InsertInventorySpecification(request);

            return Created("", new AutResultResponse() { Status = true, Data = "Success" });
        }

        private async Task<bool> CheckProductAndSpecificationIsExist(InventorySpecificationRequest request)
        {
            return await _service.CheckInventoryAndInventorySpecificationIsExist(request.ProductId,
            request.SpecificationContentId) ? true : false;
        }

        private async Task InsertInventorySpecification(InventorySpecificationRequest request)
        {
            await _service.Insert(new InventorySpecification()
            {
                InventoryId = request.ProductId,
                SpecificationContentId = request.SpecificationContentId
            });
        }
    }
}