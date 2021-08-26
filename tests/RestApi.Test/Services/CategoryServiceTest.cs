using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.IRepository;
using Services.Service;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class CategoryServiceTest
    {
        private readonly Mock<Category> _entityMock;

        private readonly Mock<ICategoryRepository> _repoMock;

        public CategoryServiceTest()
        {
            _entityMock = new Mock<Category>();
            _repoMock = new Mock<ICategoryRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new CategoryService(_repoMock.Object).Insert(_entityMock.Object));
        }
    }
}