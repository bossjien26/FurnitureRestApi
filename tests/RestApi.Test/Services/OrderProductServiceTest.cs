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
    public class OrderProductServiceTest : BaseRepositoryTest
    {
        private readonly Mock<OrderProduct> _entityMock;

        private readonly Mock<IOrderProductRepository> _repoMock;

        public OrderProductServiceTest()
        {
            _entityMock = new Mock<OrderProduct>();
            _repoMock = new Mock<IOrderProductRepository>();
        }

        [Test]
        public void ShouldInsert() => Assert.DoesNotThrowAsync(
            () => new OrderProductService(_repoMock.Object).Insert(_entityMock.Object)
        );

        [Test]
        public void ShouldGetUserOrderProductMany()
        {
            //Arrange
            var order = OrderSeeder.SeedOne();
            var orderProducts = order.OrderProducts.AsQueryable();
            _repoMock.Setup(u => u.GetAll()).Returns(orderProducts);

            //Atc
            var result = new OrderProductService(_repoMock.Object).GetUserOrderProductMany(order.Id).ToList();

            //Assert
            Assert.IsInstanceOf<List<OrderProduct>>(result);
            Assert.AreEqual(5, result.Count);
        }
    }
}