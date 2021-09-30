using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.Repositories;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class OrderPayServiceTest : BaseRepositoryTest
    {
        private readonly Mock<OrderPay> _entityMock;

        private readonly Mock<IOrderPayRepository> _repoMock;

        public OrderPayServiceTest()
        {
            _entityMock = new Mock<OrderPay>();
            _repoMock = new Mock<IOrderPayRepository>();
        }

        [Test]
        public void ShouldInsert() => Assert.DoesNotThrowAsync(
            () => new OrderPayService(_repoMock.Object).Insert(_entityMock.Object)
        );
    }
}