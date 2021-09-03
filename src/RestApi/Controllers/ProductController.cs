using DbEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.IService;
using Services.Service;

namespace RestApi.Controllers
{
    [ApiController]
    [Route('api/[Controller]')]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _repository;

        private readonly ILogger<ProductController> _logger;

        public ProductController(DbContextEntity context, ILogger<ProductController> logger)
        {
            _repository = new ProductService(context);

            _logger = logger;
        }
    }
}