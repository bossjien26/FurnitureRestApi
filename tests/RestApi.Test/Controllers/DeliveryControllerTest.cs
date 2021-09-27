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
    public class DeliveryControllerTest : BaseController
    {
        private readonly DeliveryController _controller;

        public DeliveryControllerTest()
        {
            _controller = new DeliveryController(
                _context,
                new Mock<ILogger<DeliveryController>>().Object
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
            var request = new UpdateDeliveryRequest()
            {
                Title = "DirectShipping",
                Type = DeliveryTypeEnum.DirectShipping,
                Introduce = "introduce",
                Content = "content"
            };
            await InsertDelivery();
            var response = _controller.Update(request);
            var okResult = response as OkResult;

            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        private async Task InsertDelivery()
        {
            var service = new DeliveryService(_context);
            if (service.GetDelivery(DeliveryTypeEnum.DirectShipping) == null)
            {
                await service.Insert(new Delivery()
                {
                    Title = "DirectShipping",
                    Type = DeliveryTypeEnum.DirectShipping,
                    Introduce = "test",
                    Content = "content"
                });
            }
        }
    }
}