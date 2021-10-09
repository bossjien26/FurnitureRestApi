using System.Linq;
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

        private readonly IProductService _productService;

        public OrderInventoryControllerTest()
        {
            _cartService = new CartService(_redisConnect);
            _orderService = new OrderService(_context);
            _userService = new UserService(_context);
            _orderProdcutService = new OrderInventoryService(_context);
            _productService = new ProductService(_context);
        }

        [Test]
        public async Task ShouldGetManyShowOrderInventory()
        {
            await FactoryOrderInventory();

            //Arrange & Act & Assert
            var response = _httpClient.GetAsync("/api/orderInventory/1");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);

        }


        [Test]
        public async Task ShouldInsertOrderInventory()
        {
            var order = FactoryOrderInventory();
            var response = await _httpClient.PostAsync("/api/OrderInventory", PostType(
                new CreateOrderInventoryRequest()
                {
                    orderId = order.Id
                }
            ));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<Order> FactoryOrderInventory()
        {
            var userId = _userService.SearchUserMail("jan@example.com").Id;
            var order = new Entities.Order()
            {
                UserId = userId
            };
            await _orderService.Insert(order);
            var product = ProductSeeder.SeedOne();
            await _productService.Insert(product);
            await InsertCart(userId.ToString(), product.Inventories.First().Id.ToString());
            return order;
        }

        private async Task InsertCart(string userId, string inventoryId)
        {
            var entity = new Cart();
            entity.UserId = userId;
            entity.InventoryId = inventoryId;
            entity.Quantity = "1";
            entity.Attribute = CartAttributeEnum.Shopping;
            await _cartService.Set(entity);
        }
    }
}