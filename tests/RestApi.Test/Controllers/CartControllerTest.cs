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
using RestApi.Test.DatabaseSeeders;
using Services;
using Services.Interface;
using Services.Redis;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class CartControllerTest : BaseController
    {
        private readonly IProductService _productService;

        public CartControllerTest()
        {
            _productService = new ProductService(_context);
        }

        [Test]
        public async Task ShouldStore()
        {
            var product = ProductSeeder.SeedOne();
            await _productService.Insert(product);
            //Act
            var request = new CreateCartRequest()
            {
                InventoryId = product.Inventories.First().Id,
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
                InventoryId = "2",
                Quantity = "1",
                Attribute = CartAttributeEnum.Shopping
            });
            var response = _httpClient.DeleteAsync("/api/cart?productId=2&cartAttribute=1");
            Assert.AreEqual(HttpStatusCode.NoContent, response.Result.StatusCode);
        }
    }
}