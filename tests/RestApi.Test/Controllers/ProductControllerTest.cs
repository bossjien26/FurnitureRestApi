using System;
using System.Threading.Tasks;
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

        public ProductControllerTest()
        {
            _controller = new ProductController(
                _context,
                new Mock<ILogger<ProductController>>().Object
            );
            _repository = new ProductRepository(_context);
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
        }
    }
}