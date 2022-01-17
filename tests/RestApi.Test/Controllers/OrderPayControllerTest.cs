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
        private readonly OrderService _orderService;

        private readonly IUserService _userService;

        public OrderPayControllerTest()
        {
            _orderService = new OrderService(_context);

            _userService = new UserService(_context, _redisConnect);
        }

        [Test]
        public async Task ShouldInsertOrderPay()
        {
            var order = new Entities.Order()
            {
                UserId = _userService.SearchUserMail("jan@example.com").Id
            };
            await _orderService.Insert(order);

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