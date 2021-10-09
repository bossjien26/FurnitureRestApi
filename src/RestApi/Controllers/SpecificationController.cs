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

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecificationService _service;

        private readonly ISpecificationContentService _specificationContentService;

        private readonly ILogger<SpecificationController> _logger;

        public SpecificationController(DbContextEntity context, ILogger<SpecificationController> logger)
        {
            _service = new SpecificationService(context);

            _specificationContentService = new SpecificationContentService(context);

            _logger = logger;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(RequestSpecification requestSpecification)
        {
            var specification = await InsertSpecification(requestSpecification);

            return CreatedAtAction(nameof(GetSpecification), new { id = specification.Id },
                new AutResultModel()
                {
                    Status = true,
                    Data = "Success"
                });
        }

        private async Task<Specification> InsertSpecification(RequestSpecification requestSpecification)
        {
            var specification = new Specification()
            {
                Name = requestSpecification.Name,
            };
            await _service.Insert(
                specification
            );
            return specification;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPost]
        [Route("store/specificationContent")]
        public async Task<IActionResult> storeSpecificationContent(RequestSpecificationContent requestSpecificationContent)
        {
            if (await _specificationContentService.GetById(requestSpecificationContent.SpecificationId) == null)
            {
                return NotFound(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            var specification = await InsertSpecificationContent(requestSpecificationContent);

            return CreatedAtAction(nameof(GetSpecification), new { id = specification.Id },
                new AutResultModel()
                {
                    Status = true,
                    Data = "Success"
                });
        }

        private async Task<SpecificationContent> InsertSpecificationContent(RequestSpecificationContent requestSpecificationContent)
        {
            var specification = new SpecificationContent()
            {
                Name = requestSpecificationContent.Name,
                SpecificationId = requestSpecificationContent.SpecificationId
            };
            await _specificationContentService.Insert(
                specification
            );

            return specification;
        }

        [Route("{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage)
        {
            return Ok(_service.GetMany(perPage, 10).ToList());
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Specification>> GetSpecification(int id)
        {
            var specification = await _service.GetById(id);
            return Ok(specification);
        }
    }
}