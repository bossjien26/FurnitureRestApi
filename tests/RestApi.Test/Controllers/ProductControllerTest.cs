using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models.Requests;
using Services.Interface;
using Services;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class ProductControllerTest : BaseController
    {
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _controller = new ProductController(
                _context,
                new Mock<ILogger<ProductController>>().Object
            );
        }

        [Test]
        public async Task ShouldInsertProduct()
        {
            var request = new RequestProduct()
            {
                Name = "name"
            };

            var response = await _httpClient.PostAsync("/api/product", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task ShouldInsertProductCategory()
        {
            ICategoryService categoryService = new CategoryService(_context);
            IProductService productService = new ProductService(_context);
            var request = new RequestProductCategory()
            {
                ProductId = productService.GetAll().OrderByDescending(x => x.Id).First().Id,
                CategoryId = categoryService.GetMany(1, 1).First().Id
            };

            var response = await _httpClient.PostAsync("/api/product/store/productCategory", PostType(request));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}