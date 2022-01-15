using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.DatabaseSeeders;
using Services;
using Services.Dto;

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

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(CategorySeeder.SeedMany(10, 15).AsQueryable());

            var result = new CategoryService(_repoMock.Object).GetMany(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Category>>(result);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void ShouldGetGetChildren()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(CategorySeeder.SeedMany(10, 15).AsQueryable());

            var result = new CategoryService(_repoMock.Object).GetChildren(_entityMock.Object.Id).ToList();

            //Assert
            Assert.IsInstanceOf<List<Category>>(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void ShouldGetGetCategoryRelationChildren()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(CategorySeeder.SeedMany(10, 15).AsQueryable());

            var result = new CategoryService(_repoMock.Object).GetCategoryRelationChildren(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<CategoryRelationChildren>>(result);
            Assert.AreEqual(5, result.Count);
        }

    }
}