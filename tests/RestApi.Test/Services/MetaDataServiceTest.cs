using System.Threading.Tasks;
using Entities;
using Enum;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class MetadataServiceTest
    {
        private readonly Mock<Metadata> _entityMock;

        private readonly Mock<IMetadataRepository> _repoMock;

        public MetadataServiceTest()
        {
            _entityMock = new Mock<Metadata>();
            _repoMock = new Mock<IMetadataRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new MetadataService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new MetadataService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<Metadata>(result);
        }

        [Test]
        public void ShouldUpdate()
        {
            //Arrange
            //Act
            //Asset
            _repoMock.Setup(c => c.Get(x => x.Key == 1))
                .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.Key = 2;

            Assert.DoesNotThrow(() =>
                new MetadataService(_repoMock.Object).Update(_entityMock.Object)
            );
        }

        [Test]
        public void ShouldGetByCategory()
        {
            _repoMock.Setup(c => c.Get(x => x.Key == (int)PaymentTypeEnum.Bank
            && x.Category == MetadataCategoryEnum.Pay))
            .Returns(Task.FromResult(_entityMock.Object));


            Assert.DoesNotThrow(() =>
                new MetadataService(_repoMock.Object)
            .GetByCategoryDetail(MetadataCategoryEnum.Pay, (int)PaymentTypeEnum.Bank)
            );
        }
    }
}