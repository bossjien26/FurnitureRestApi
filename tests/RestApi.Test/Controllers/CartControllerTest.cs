using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models.Requests;

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
            var requestCart = new RequestCart()
            {
                ProductId = 2,
                Quantity = 1,
                Attribute = CartAttribute.Shopping
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(requestCart).ToString(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5002/api/cart/Store", content);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //TODO:test delete
        [Test]
        public void ShouldDelete()
        {
            var response = _httpClient.DeleteAsync("http://localhost:5002/api/cart/delete?productId=2&cartAttribute=1");
            Assert.AreEqual(HttpStatusCode.NoContent, response.Result.StatusCode);
        }
    }
}