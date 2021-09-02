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
    public class SpecificationContentServiceTest
    {
        private readonly Mock<SpecificationContent> _entityMock;
        private readonly Mock<ISpecificationContentRepository> _repoMock;

        public SpecificationContentServiceTest()
        {
            _entityMock = new Mock<SpecificationContent>();

            _repoMock = new Mock<ISpecificationContentRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(() => new SpecificationContentService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(SpecificationSeeder.SeedSpecificationContentMany(10,15).AsQueryable());

            var result = new SpecificationContentService(_repoMock.Object).GetMany(1,5).ToList();

            //Assert
            Assert.IsInstanceOf<List<SpecificationContent>>(result);
            Assert.AreEqual(5,result.Count);
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new SpecificationContentService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<SpecificationContent>(result);
        }
    }
}