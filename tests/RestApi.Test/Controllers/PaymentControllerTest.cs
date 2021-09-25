using System.Net;
using System.Threading.Tasks;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models.Requests;
using Services;
using Services.Dto;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class PaymentControllerTest : BaseController
    {
        private readonly PaymentController _controller;

        public PaymentControllerTest()
        {
            _controller = new PaymentController(
                _context,
                new Mock<ILogger<PaymentController>>().Object
            );
        }

        [Test]
        public void ShouldShowMany()
        {
            var result = _controller.ShowMany();
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var request = new UpdatePaymentRequest()
            {
                Title = "Bank",
                Type = PaymentTypeEnum.Bank,
                Introduce = "introduce",
                Content = "content"
            };
            await InsertPayment();
            var response = _controller.Update(request);
            var okResult = response as OkResult;

            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        private async Task InsertPayment()
        {
            var service = new PaymentService(_context);
            if (service.GetPayment(PaymentTypeEnum.Bank) == null)
            {
                await service.Insert(new Payment()
                {
                    Title = "Bank",
                    Type = PaymentTypeEnum.Bank,
                    Introduce = "test",
                    Content = "content"
                });
            }
        }
    }
}