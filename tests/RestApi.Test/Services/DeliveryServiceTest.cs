using System.Threading.Tasks;
using Entities;
using Enum;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.Repositories;
using Services;
using Services.Dto;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class DeliveryServiceTest : BaseRepositoryTest
    {
        private readonly Mock<MetaData> _entityMock;

        private readonly Mock<IMetaDataRepository> _repoMock;

        private readonly Mock<Delivery> _delivery;

        public DeliveryServiceTest()
        {
            _entityMock = new Mock<MetaData>();
            _repoMock = new Mock<IMetaDataRepository>();
            _delivery = new Mock<Delivery>();
        }

        [Test]
        public void ShouldUpdate()
        {
            _repoMock.Setup(c => c.Get(x => x.Value == "category"))
                            .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.Value = "test";

            Assert.DoesNotThrow(() =>
                new DeliveryService(_context).Update(_delivery.Object)
            );
        }

        [Test]
        public void ShouldGetdelivery()
        {
            _repoMock.Setup(c => c.Get(x => x.Key == DeliveryTypeEnum.DirectShipping.ToString()
            && x.Category == MetaDataCategoryEnum.Delivery))
            .Returns(Task.FromResult(_entityMock.Object));


            Assert.DoesNotThrow(() =>
                new DeliveryService(_context)
                .GetDelivery(DeliveryTypeEnum.DirectShipping)
            );
        }
    }
}