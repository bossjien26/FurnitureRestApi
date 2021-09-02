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

        [Authorize(Role.SuperAdmin, Role.Customer, Role.Admin, Role.Staff)]
        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> Insert(RequestSpecification requestSpecification)
        {
            await InsertSpecification(requestSpecification);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task InsertSpecification(RequestSpecification requestSpecification)
        {
            await _repository.Insert(
                new Specification()
                {
                    Name = requestSpecification.Name,
                }
            );
        }

        [Authorize(Role.SuperAdmin, Role.Customer, Role.Admin, Role.Staff)]
        [HttpPost]
        [Route("insertunderlayer")]
        public async Task<IActionResult> InsertUnderLayer(RequestSpecificationContent requestSpecificationContent)
        {
            if (await _repositorySpecificationContent.GetById(requestSpecificationContent.SpecificationId) == null)
            {
                return Ok(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await InsertSpecification(requestSpecificationContent);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task InsertSpecification(RequestSpecificationContent requestSpecificationContent)
        {
            await _repositorySpecificationContent.Insert(
                new SpecificationContent()
                {
                    Name = requestSpecificationContent.Name,
                    SpecificationId = requestSpecificationContent.SpecificationId
                }
            );
        }

        [Authorize(Role.SuperAdmin, Role.Admin, Role.Staff)]
        [Route("many")]
        [HttpGet]
        public IActionResult ShowMany(int pages)
        {
            return Ok(_repository.GetMany(pages, 10).ToList());
        }
    }
}