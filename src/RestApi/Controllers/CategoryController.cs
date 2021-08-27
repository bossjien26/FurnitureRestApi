using System.Threading.Tasks;
using DbEntity;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.src.Models;
using Services.IService;
using Services.Service;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/Controller")]
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
        [Route("Insert")]
        [HttpPost]
        public async Task<IActionResult> Insert(Models.Requests.Category category)
        {
            if (await _repository.GetById(category.ChildrenId) == null && category.ChildrenId != 0)
            {
                return Ok(new AutResultModel()
                {
                    Status = false,
                    Data = "Fail"
                });
            }

            await InsertCategory(category);

            return Ok(new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task InsertCategory(Models.Requests.Category category)
        {
            await _repository.Insert(
                new Entities.Category()
                {
                    Name = category.Name,
                    ChildrenId = category.ChildrenId,
                    Sequence = category.Sequence,
                    IsDisplay = category.IsDisplay
                }
            );
        }
    }
}