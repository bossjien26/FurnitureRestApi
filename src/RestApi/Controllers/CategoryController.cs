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
                return NotFound(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            var category = await InsertCategory(requestsCategory);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _repository.GetById(id);
            return Ok(category);
        }

        private async Task<Category> InsertCategory(RequestCategory requestsCategory)
        {
            var category = new Entities.Category()
            {
                Name = requestsCategory.Name,
                ChildrenId = requestsCategory.ChildrenId,
                Sequence = requestsCategory.Sequence,
                IsDisplay = requestsCategory.IsDisplay
            };
            await _repository.Insert(category);
            return category;
        }

        [Authorize(Role.SuperAdmin, Role.Admin, Role.Staff)]
        [Route("showMany/{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage)
        {
            return Ok(_repository.GetMany(perPage, 10).ToList());
        }
    }
}