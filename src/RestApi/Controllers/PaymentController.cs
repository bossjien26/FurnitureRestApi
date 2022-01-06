using DbEntity;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using Services;
using Services.Dto;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        private readonly IPaymentService _service;

        public PaymentController(DbContextEntity context,
        ILogger<PaymentController> logger)
        {
            _service = new PaymentService(context);
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous()]
        public IActionResult ShowMany()
        {
            return Ok(_service.GetMany());
        }

        [Authorize(RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPut]
        [Route("")]
        public IActionResult Update(UpdatePaymentRequest request)
        {
            var payment = new Payment()
            {
                Content = request.Content,
                Title = request.Title,
                Type = request.Type,
                Introduce = request.Introduce
            };
            _service.Update(payment);
            return Ok();
        }
    }
}