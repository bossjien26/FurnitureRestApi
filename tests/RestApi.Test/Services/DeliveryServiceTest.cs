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
        private readonly Mock<Metadata> _entityMock;

        private readonly Mock<IMetadataRepository> _repoMock;

        private readonly Mock<Delivery> _delivery;

        public DeliveryServiceTest()
        {
            _entityMock = new Mock<Metadata>();
            _repoMock = new Mock<IMetadataRepository>();
            _delivery = new Mock<Delivery>();
        }

        [Test]
        public void ShouldUpdate()
        {
            _repoMock.Setup(c => c.Get(x => x.Value == "category"))
                            .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.Value = "test";

            Assert.DoesNotThrowAsync(() =>
                new DeliveryService(_context).Update(_delivery.Object)
            );
        }

        [Test]
        public void ShouldGetdelivery()
        {
            _repoMock.Setup(c => c.Get(x => x.Key == (int)DeliveryTypeEnum.DropShipping
            && x.Category == MetadataCategoryEnum.Delivery))
            .Returns(Task.FromResult(_entityMock.Object));


            Assert.DoesNotThrowAsync(() =>
                new DeliveryService(_context)
                .GetDelivery(DeliveryTypeEnum.DropShipping)
            );
        }

        [Test]
        public void ShouldInsert()
        {
            _delivery.Object.Title = "直接運送";
            _delivery.Object.Introduce = "您再也無需提前支付庫存費用或處理運送物流。使用直運(Dropshipping) 後，便可透過批發商直接將產品寄給顧客。";
            _delivery.Object.Content = "內容";
            _delivery.Object.Type = DeliveryTypeEnum.DropShipping;
            Assert.DoesNotThrowAsync(()
                => new DeliveryService(_context).Insert(_delivery.Object));
        }

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(c => c.Get(x => x.Key == (int)DeliveryTypeEnum.DropShipping))
            .Returns(Task.FromResult(_entityMock.Object));

            Assert.DoesNotThrow(() =>
                new DeliveryService(_context)
                .GetMany()
            );

        }
    }
}