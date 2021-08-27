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

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new CategoryService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<Category>(result);
        }
    }
}