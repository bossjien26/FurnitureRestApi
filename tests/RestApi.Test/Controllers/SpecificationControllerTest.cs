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
    public class SpecificationControllerTest : BaseRepositoryTest
    {
        private readonly SpecificationController _controller;

        private readonly ISpecificationRepository _repository;

        public SpecificationControllerTest()
        {
            _controller = new SpecificationController(
                _context,
                new Mock<ILogger<SpecificationController>>().Object
            );
            _repository = new SpecificationRepository(_context);
        }

        [Test]
        public async Task ShouldInsertSpecification()
        {
            var result = await _controller.Insert(new RequestSpecification()
            {
                Name = "123"
            });
        }

        [Test]
        public async Task ShouldInsertSpecificationContent()
        {
            var result = await _controller.InsertUnderLayer(new RequestSpecificationContent()
            {
                Name = "123",
                SpecificationId = 1
            });
        }
    }
}