using System.Net;
using System.Threading.Tasks;
using Enum;
using NUnit.Framework;
using RestApi.Models.Requests;
using Services;
using Services.Interface;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class OrderPayControllerTest : BaseController
    {
        [Test]
        public async Task ShouldInsertOrderPay()
        {
            IOrderService orderService = new OrderService(_context);
            IUserService userService = new UserService(_context);

            var order = new Entities.Order()
            {
                UserId = userService.SearchUserMail("jan@example.com").Id
            };
            await orderService.Insert(order);

            var request = new CreateOrderPayRequest()
            {
                orderId = order.Id,
                Terms = PaymentTypeEnum.Bank
            };

            var response = await _httpClient.PostAsync("/api/orderPay", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}