using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.DatabaseSeeders;
using RestApi.Test.Repositories;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class OrderInventoryServiceTest : BaseRepositoryTest
    {
        private readonly Mock<OrderInventory> _entityMock;

        private readonly Mock<IOrderInventoryRepository> _repoMock;

        public OrderInventoryServiceTest()
        {
            _entityMock = new Mock<OrderInventory>();
            _repoMock = new Mock<IOrderInventoryRepository>();
        }

        [Test]
        public void ShouldInsert() => Assert.DoesNotThrowAsync(
            () => new OrderInventoryService(_repoMock.Object).Insert(_entityMock.Object)
        );

        [Test]
        public void ShouldGetUserOrderInventoryMany()
        {
            //Arrange
            var order = OrderSeeder.SeedOne(ProductSeeder.SeedOne().Inventories.First().Id);
            var orderInventories = order.OrderInventories.AsQueryable();
            _repoMock.Setup(u => u.GetAll()).Returns(orderInventories);

            //Atc
            var result = new OrderInventoryService(_repoMock.Object).GetUserOrderInventoryMany(order.Id).ToList();

            //Assert
            Assert.IsInstanceOf<List<OrderInventory>>(result);
            Assert.AreEqual(5, result.Count);
        }
    }
}