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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(DbContextEntity context, ILogger<CategoryController> logger)
        {
            _service = new CategoryService(context);

            _logger = logger;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Insert(CreateCategoryRequest requestsCategory)
        {
            if (await _service.GetById(requestsCategory.ParentId) == null && requestsCategory.ParentId != 0)
            {
                return NotFound(new AutResultResponse()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            var category = await InsertCategory(requestsCategory);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new AutResultResponse()
            {
                Status = true,
                Data = "Success"
            });
        }

        [HttpGet]
        [Route("show/{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _service.GetById(id);
            return Ok(category);
        }

        private async Task<Category> InsertCategory(CreateCategoryRequest requestsCategory)
        {
            var category = new Entities.Category()
            {
                Name = requestsCategory.Name,
                ParentId = requestsCategory.ParentId,
                Sequence = requestsCategory.Sequence,
                IsDisplay = requestsCategory.IsDisplay
            };
            await _service.Insert(category);
            return category;
        }

        [Route("{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage)
        => Ok(_service.GetCategoryRelationChildren(perPage, 10));
    }
}