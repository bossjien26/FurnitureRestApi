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
    public class SpecificationServiceTest
    {
        private readonly Mock<Specification> _entityMock;
        private readonly Mock<ISpecificationRepository> _repoMock;

        public SpecificationServiceTest()
        {
            _entityMock = new Mock<Specification>();

            _repoMock = new Mock<ISpecificationRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(() => new SpecificationService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(SpecificationSeeder.SeedMany(10,15).AsQueryable());

            var result = new SpecificationService(_repoMock.Object).GetMany(1,5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Specification>>(result);
            Assert.AreEqual(5,result.Count);
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new SpecificationService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<Specification>(result);
        }
    }
}