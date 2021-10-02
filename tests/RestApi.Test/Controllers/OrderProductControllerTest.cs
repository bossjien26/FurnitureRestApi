using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RestApi.Models.Requests;
using RestApi.Test.DatabaseSeeders;
using Services;
using Services.Interface;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class OrderProductControllerTest : BaseController
    {
        [Test]
        public async Task ShouldInsertOrderProduct()
        {
            IOrderService orderService = new OrderService(_context);
            IUserService userService = new UserService(_context);
            IOrderProductService orderProdcutService = new OrderProductService(_context);
            IProductService prodcutService = new ProductService(_context);

            var order = new Entities.Order()
            {
                UserId = userService.SearchUserMail("jan@example.com").Id
            };
            await orderService.Insert(order);
            var product = ProductSeeder.SeedOne();
            await prodcutService.Insert(product);
            var request = new CreateOrderProductRequest()
            {
                orderId = order.Id,
                productList = new System.Collections.Generic.List<CreateOrderProductListRequest>(){
                    new CreateOrderProductListRequest(){
                        ProductId = product.Id,
                        Specification = "",
                        Quality = 1
                    }
                }
            };

            var response = await _httpClient.PostAsync("/api/orderProduct", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}