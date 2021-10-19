using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models.Requests;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class CategoryControllerTest : BaseController
    {
        private readonly CategoryController _controller;

        public CategoryControllerTest()
        {
            _controller = new CategoryController(
                _context,
                new Mock<ILogger<CategoryController>>().Object
            );
        }

        [Test]
        public async Task ShouldInsertCategory()
        {
            var request = new CreateCategoryRequest()
            {
                Name = "123",
                ChildrenId = 0,
                IsDisplay = false
            };
            var response = await _httpClient.PostAsync("/api/category", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}