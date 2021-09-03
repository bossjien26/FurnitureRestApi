using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Repositories.IRepository;
using Repositories.Repository;
using RestApi.Controllers;
using RestApi.Models.Requests;
using RestApi.Test.Repositories;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class ProductControllerTest : BaseRepositoryTest
    {
        private readonly ProductController _controller;

        private readonly IProductRepository _repository;

        private readonly IProductCategoryRepository _categoryRepository;

        public ProductControllerTest()
        {
            _controller = new ProductController(
                _context,
                new Mock<ILogger<ProductController>>().Object
            );
            _repository = new ProductRepository(_context);
            _categoryRepository = new ProductCategoryRepository(_context);
        }

        [Test]
        public async Task ShouldInsertProduct()
        {
            var result = await _controller.Insert(new RequestProduct()
            {
                Name = "name",
                Price = 1,
                Sequence = 1,
                Quantity = 1,
                RelateAt = DateTime.Now
            });

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShouldInsertProductCategoryIsFail()
        {
            var result = await _controller.StoreProductCategory(new RequestProductCategory()
            {
                ProductId = 1,
                CategoryId = 1
            });

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShouldInsertProductSpecificationIsFail()
        {
            var result = await _controller.StoreProductSpecification(new RequestProductSpecification()
            {
                ProductId = 1,
                SpecificationId = 1
            });

            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}