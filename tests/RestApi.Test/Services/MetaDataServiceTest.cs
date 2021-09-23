using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class MetaDataServiceTest
    {
        private readonly Mock<MetaData> _entityMock;

        private readonly Mock<IMetaDataRepository> _repoMock;

        public MetaDataServiceTest()
        {
            _entityMock = new Mock<MetaData>();
            _repoMock = new Mock<IMetaDataRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new MetaDataService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new MetaDataService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<MetaData>(result);
        }

        [Test]
        public void ShouldUpdate()
        {
            //Arrange
            //Act
            //Asset
            _repoMock.Setup(c => c.Get(x => x.Key == "category"))
                .Returns(Task.FromResult(_entityMock.Object));

            _entityMock.Object.Key = "cart";

            Assert.DoesNotThrow(() =>
                new MetaDataService(_repoMock.Object).Update(_entityMock.Object)
            );
        }
    }
}