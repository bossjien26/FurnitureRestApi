using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models.Requests;
using Services;
using Services.Redis;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class CartControllerTest : BaseController
    {
        private readonly CartController _controller;

        public CartControllerTest()
        {
            _controller = new CartController(
                _context,
                new Mock<ILogger<CartController>>().Object,
                new Mock<IHttpContextAccessor>().Object,
                _redisConnect
            );
        }

        [Test]
        public async Task ShouldStore()
        {
            //Act
            var request = new RequestCart()
            {
                ProductId = 2,
                Quantity = 1,
                Attribute = CartAttributeEnum.Shopping
            };
            var response = await _httpClient.PostAsync("/api/cart", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task ShouldDelete()
        {
            var service = new CartService(_redisConnect);
            var userService = new UserService(_context);
            var user = userService.GetAll().Where(u => u.Mail == "jan@example.com").First();
            await service.Set(new Entities.Cart()
            {
                UserId = user.Id.ToString(),
                ProductId = "2",
                Quantity = "1",
                Attribute = CartAttributeEnum.Shopping
            });
            var response = _httpClient.DeleteAsync("/api/cart?productId=2&cartAttribute=1");
            Assert.AreEqual(HttpStatusCode.NoContent, response.Result.StatusCode);
        }
    }
}