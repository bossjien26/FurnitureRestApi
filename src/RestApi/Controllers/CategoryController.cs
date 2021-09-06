using System.Linq;
using System.Threading.Tasks;
using DbEntity;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _repository;

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(DbContextEntity context, ILogger<CategoryController> logger)
        {
            _repository = new CategoryService(context);

            _logger = logger;
        }

        [Authorize(Role.SuperAdmin, Role.Admin)]
        [Route("insert")]
        [HttpPost]
        public async Task<IActionResult> Insert(RequestCategory requestsCategory)
        {
            if (await _repository.GetById(requestsCategory.ChildrenId) == null && requestsCategory.ChildrenId != 0)
            {
                return Ok(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await InsertCategory(requestsCategory);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task InsertCategory(RequestCategory requestsCategory)
        {
            await _repository.Insert(
                new Entities.Category()
                {
                    Name = requestsCategory.Name,
                    ChildrenId = requestsCategory.ChildrenId,
                    Sequence = requestsCategory.Sequence,
                    IsDisplay = requestsCategory.IsDisplay
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