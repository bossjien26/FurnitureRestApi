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
    public class CategoryControllerTest : BaseRepositoryTest
    {
        private readonly CategoryController _controller;

        private readonly ICategoryRepository _repository;

        public CategoryControllerTest()
        {
            _controller = new CategoryController(
                _context,
                new Mock<ILogger<CategoryController>>().Object
            );
            _repository = new CategoryRepository(_context);
        }

        [Test]
        public async Task ShouldInsertCategory()
        {
            var result = await _controller.Insert(new RequestCategory(){
                Name = "123",
                ChildrenId = 0,
                IsDisplay = false
            });
        }
    }
}