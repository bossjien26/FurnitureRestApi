using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestApi.Models.Requests;
using Services;
using Services.Interface;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class OrderControllerTest : BaseController
    {
        private readonly IOrderService _orderService;

        private readonly IUserService _userService;

        public OrderControllerTest()
        {
            _orderService = new OrderService(_context);

            _userService = new UserService(_context, _redisConnect);
        }

        [Test]
        public async Task ShouldStore()
        {
            //Act
            var request = new CreateOrderRequest()
            {
                Country = "country",
                City = "city",
                Street = "street",
                Recipient = "recipient",
                RecipientMail = _testMail,
                RecipientPhone = "123456789;",
                Sender = "sender"
            };
            var response = await _httpClient.PostAsync("/api/order", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task ShouldGetUserOrder()
        {
            var orderId = _orderService.GetUserOrderMany(_userService.SearchUserMail("jan@example.com").Id, 1, 5).First().Id;
            var response = await _httpClient.GetAsync("/api/order/show/" + orderId);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldGetUserOrderMany()
        {
            var response = await _httpClient.GetAsync("/api/order/1");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var orderId = _orderService.GetUserOrderMany(_userService.SearchUserMail("jan@example.com").Id, 1, 5).First().Id;
            //Act
            var request = new UpdateOrderRequest()
            {
                OrderId = orderId,
                Country = "country",
                City = "city",
                Street = "street",
                Recipient = "recipient",
                RecipientMail = _testMail,
                RecipientPhone = "123456789;",
                Sender = "senderaa"
            };
            var response = await _httpClient.PutAsync("/api/order", PostType(request));

            // //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}