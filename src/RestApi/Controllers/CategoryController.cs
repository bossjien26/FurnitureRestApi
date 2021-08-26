using DbEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    }
}