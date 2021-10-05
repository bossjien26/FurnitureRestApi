using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.DatabaseSeeders;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class InventoryServiceTest
    {
        private readonly Mock<Inventory> _entityMock;

        private readonly Mock<IInventoryRepository> _repoMock;

        public InventoryServiceTest()
        {
            _entityMock = new Mock<Inventory>();
            _repoMock = new Mock<IInventoryRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new InventoryService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));
            var result = await new InventoryService(_repoMock.Object).GetById(1);
            Assert.IsInstanceOf<Inventory>(result);
        }

        [Test]
        public async Task ShouldGetShowById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1 && x.IsDisplay == true && x.IsDelete == false))
            .Returns(Task.FromResult(_entityMock.Object));
            var result = await new InventoryService(_repoMock.Object).GetShowById(1);
            Assert.IsInstanceOf<Inventory>(result);
        }

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(ProductSeeder.SeedOne().Inventories.AsQueryable());
            var result = new InventoryService(_repoMock.Object).GetMany(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Inventory>>(result);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void ShouldGetShowMany()
        {
            var inventory =  ProductSeeder.SeedOne().Inventories;
            _repoMock.Setup(r => r.GetAll()).Returns(ProductSeeder.SeedOne().Inventories.AsQueryable());
            var result = new InventoryService(_repoMock.Object).GetShowMany(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Inventory>>(result);
            Assert.AreEqual(5, result.Count);
        }
    }
}