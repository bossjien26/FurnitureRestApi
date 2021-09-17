using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.Controllers;
using RestApi.Models.Requests;
using Services.Service;

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
                Name = "name",
                Price = 1,
                Sequence = 1,
                Quantity = 1,
                RelateAt = DateTime.Now
            };

            var response = await _httpClient.PostAsync("http://localhost:5002/api/product/insert", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task ShouldInsertProductCategory()
        {
            var categoryService = new CategoryService(_context);
            var productService = new ProductService(_context);
            var request = new RequestProductCategory()
            {
                ProductId = productService.GetAll().OrderByDescending(x => x.Id).First().Id,
                CategoryId = categoryService.GetMany(1, 1).First().Id
            };

            var response = await _httpClient.PostAsync("http://localhost:5002/api/product/insertProductCategory", PostType(request));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task ShouldInsertProductSpecification()
        {
            var productService = new ProductService(_context);
            var specificationService = new SpecificationService(_context);

            var request = new RequestProductSpecification()
            {
                ProductId = productService.GetAll().OrderByDescending(x => x.Id).First().Id,
                SpecificationId = specificationService.GetMany(1, 1).First().Id
            };

            var response = await _httpClient.PostAsync("http://localhost:5002/api/product/insertProductSpecification", PostType(request));
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}