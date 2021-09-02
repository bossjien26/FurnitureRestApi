using DbEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.IService;
using Services.Service;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/controller")]
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
    }
}