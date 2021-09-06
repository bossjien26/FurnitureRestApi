using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.IRepository;
using RestApi.Test.DatabaseSeeders;
using Services.Service;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class CartServiceTest
    {
        private readonly Mock<Cart> _entityMock;

        private readonly Mock<ICartRepository> _repoMock;

        public CartServiceTest()
        {
            _entityMock = new Mock<Cart>();
            _repoMock = new Mock<ICartRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new CartService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new CartService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<Cart>(result);
        }

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(CartSeeder.SeedMany(10, 15).AsQueryable());

            var result = new CartService(_repoMock.Object).GetMany(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Cart>>(result);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void ShouldUpdate()
        {
            //Arrange
            //Act
            //Asset
            _repoMock.Setup(c => c.Get(x => x.Quantity == 1))
                .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.Quantity = 2;

            Assert.DoesNotThrow(() =>
                new CartService(_repoMock.Object).Update(_entityMock.Object)
            );
        }

        [Test]
        public void ShouldDelete()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            Assert.DoesNotThrow(() =>
                new CartService(_repoMock.Object).Delete(_entityMock.Object)
            );
        }

        [Test]
        public async Task ShouldGetUserCart()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1 && x.UserId == 1 && x.ProductId == 1))
            .Returns(Task.FromResult(_entityMock.Object));

            var result = await new CartService(_repoMock.Object).GetUserCart(1,1,1);

            Assert.IsInstanceOf<Cart>(result);
        }
    }
}