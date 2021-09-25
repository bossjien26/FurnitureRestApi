using System.Collections.Generic;
using DbEntity;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        [Route("index")]
        public IActionResult Index()
        {
            return Ok(GetPaymentMany());
        }

        private List<Payment> GetPaymentMany()
        {
            var bank = _service.GetPayment(PaymentTypeEnum.Bank);
            var COD = _service.GetPayment(PaymentTypeEnum.COD);
            var credit = _service.GetPayment(PaymentTypeEnum.Credit);

            var payments = new List<Payment>() { };
            payments.Add(bank);
            payments.Add(COD);
            payments.Add(credit);

            return payments;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        public IActionResult Update(UpdatePaymentRequest request)
        {
            var payment = _service.GetPayment(request.Type);
            _service.Update(payment);
            return Ok();
        }
    }
}