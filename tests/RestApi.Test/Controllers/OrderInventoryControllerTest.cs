using System.Net;
using System.Threading.Tasks;
using Entities;
using Enum;
using NUnit.Framework;
using RestApi.Models.Requests;
using RestApi.Test.DatabaseSeeders;
using Services;
using Services.Interface;
using Services.Interface.Redis;
using Services.Redis;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class OrderInventoryControllerTest : BaseController
    {
        private readonly ICartService _cartService;

        private readonly IOrderService _orderService;

        private readonly IUserService _userService;

        private readonly IOrderInventoryService _orderProdcutService;

        private readonly IProductService _prodcutService;

        public OrderInventoryControllerTest()
        {
            _cartService = new CartService(_redisConnect);
            _orderService = new OrderService(_context);
            _userService = new UserService(_context);
            _orderProdcutService = new OrderInventoryService(_context);
            _prodcutService = new ProductService(_context);
        }

        [Test]
        public async Task ShouldInsertOrderInventory()
        {
            var userId = _userService.SearchUserMail("jan@example.com").Id;
            var order = new Entities.Order()
            {
                UserId = userId
            };
            await _orderService.Insert(order);
            var product = ProductSeeder.SeedOne();
            await _prodcutService.Insert(product);
            await InsertCart(userId.ToString(), product.Id.ToString());
            var response = await _httpClient.PostAsync("/api/OrderInventory", PostType(
                new CreateOrderInventoryRequest()
                {
                    orderId = order.Id
                }
            ));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task InsertCart(string userId, string productId)
        {
            var entity = new Cart();
            entity.UserId = userId;
            entity.ProductId = productId;
            entity.Quantity = "1";
            entity.Attribute = CartAttributeEnum.Shopping;
            await _cartService.Set(entity);
        }
    }
}