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
    public class OrderServiceTest : BaseRepositoryTest
    {
        private readonly Mock<Order> _entityMock;

        private readonly Mock<IOrderRepository> _repoMock;

        private readonly Mock<Product> _productEntityMock;

        private readonly Mock<IProductRepository> _productRepoMock;

        public OrderServiceTest()
        {
            _entityMock = new Mock<Order>();
            _repoMock = new Mock<IOrderRepository>();
            _productEntityMock = new Mock<Product>();
            _productRepoMock = new Mock<IProductRepository>();
        }

        [Test]
        public void ShouldInsert() => Assert.DoesNotThrowAsync(
            () => new OrderService(_repoMock.Object).Insert(_entityMock.Object)
        );

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new OrderService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<Order>(result);
        }

        [Test]
        public void ShouldUpdate()
        {
            //Arrange
            //Act
            //Asset
            _repoMock.Setup(c => c.Get(x => x.RecipientMail == "example@example.com"))
                .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.RecipientMail = "a@example.com";

            Assert.DoesNotThrow(() =>
                new OrderService(_repoMock.Object).Update(_entityMock.Object)
            );
        }

        [Test]
        public void ShouldGetMany()
        {
            //Arrange
            var order = OrderSeeder.SeedMany(10, 15).AsQueryable();
            _repoMock.Setup(u => u.GetAll()).Returns(order);

            //Atc
            var result = new OrderService(_repoMock.Object).GetMany(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Order>>(result);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public async Task ShouldGetUserOrder()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1 && x.UserId == 1)).Returns(Task.FromResult(_entityMock.Object));
            var result = await new OrderService(_repoMock.Object).GetUserOrder(1, 1);
            Assert.IsInstanceOf<Order>(result);
        }

        [Test]
        public void ShouldGetUserOrderMany()
        {
            var product = ProductSeeder.SeedMany(1, 10).AsQueryable();
            _productRepoMock.Setup(u => u.GetAll()).Returns(product);


            //Arrange
            var order = OrderSeeder.SeedUserMany(1, product.First().Inventories.First().Id, 10, 15).AsQueryable();
            _repoMock.Setup(u => u.GetAll()).Returns(order);

            //Atc
            var result = new OrderService(_repoMock.Object).GetUserOrderMany(1, 1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Order>>(result);
            Assert.AreEqual(5, result.Count);
        }
    }
}