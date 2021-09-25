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
        private readonly ISpecificationService _repository;

        private readonly ISpecificationContentService _repositorySpecificationContent;

        private readonly ILogger<SpecificationController> _logger;

        public SpecificationController(DbContextEntity context, ILogger<SpecificationController> logger)
        {
            _repository = new SpecificationService(context);

            _repositorySpecificationContent = new SpecificationContentService(context);

            _logger = logger;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Customer, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPost]
        [Route("insert")]
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
            await _repository.Insert(
                specification
            );
            return specification;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Customer, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPost]
        [Route("insertSpecificationContent")]
        public async Task<IActionResult> storeSpecificationContent(RequestSpecificationContent requestSpecificationContent)
        {
            if (await _repositorySpecificationContent.GetById(requestSpecificationContent.SpecificationId) == null)
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
            await _repositorySpecificationContent.Insert(
                specification
            );

            return specification;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [Route("showMany/{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage)
        {
            return Ok(_repository.GetMany(perPage, 10).ToList());
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Specification>> GetSpecification(int id)
        {
            var specification = await _repository.GetById(id);
            return Ok(specification);
        }
    }
}