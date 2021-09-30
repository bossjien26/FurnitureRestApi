using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
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
    }
}