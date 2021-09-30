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
                RecipientMail = "example@example.com",
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
            IOrderService orderService = new OrderService(_context);
            IUserService userService = new UserService(_context);
            var orderId = orderService.GetUserOrderMany(userService.SearchUserMail("jan@example.com").Id, 1, 5).First().Id;
            var response = await _httpClient.GetAsync("/api/order/show/" + orderId);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldGetUserOrderMany()
        {
            IUserService userService = new UserService(_context);
            var response = await _httpClient.GetAsync("/api/order/1");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldUpdate()
        {
            IOrderService orderService = new OrderService(_context);
            IUserService userService = new UserService(_context);
            var orderId = orderService.GetUserOrderMany(userService.SearchUserMail("jan@example.com").Id, 1, 5).First().Id;
            //Act
            var request = new UpdateOrderRequest()
            {
                OrderId = orderId,
                Country = "country",
                City = "city",
                Street = "street",
                Recipient = "recipient",
                RecipientMail = "example@example.com",
                RecipientPhone = "123456789;",
                Sender = "senderaa"
            };
            var response = await _httpClient.PutAsync("/api/order", PostType(request));

            // //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}