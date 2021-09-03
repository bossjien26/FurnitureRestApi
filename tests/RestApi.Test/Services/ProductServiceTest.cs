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
    public class ProductServiceTest
    {
        private readonly Mock<Product> _entityMock;

        private readonly Mock<IProductRepository> _repoMock;

        public ProductServiceTest()
        {
            _entityMock = new Mock<Product>();
            _repoMock = new Mock<IProductRepository>();
        }

        [Test]
        public void ShouldInsert()
        {
            Assert.DoesNotThrowAsync(()
                => new ProductService(_repoMock.Object).Insert(_entityMock.Object));
        }

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = await new ProductService(_repoMock.Object).GetById(1);

            Assert.IsInstanceOf<Product>(result);
        }

        [Test]
        public void ShouldGetMany()
        {
            _repoMock.Setup(r => r.GetAll()).Returns(ProductSeeder.SeedMany(10, 15).AsQueryable());

            var result = new ProductService(_repoMock.Object).GetMany(1, 5).ToList();

            //Assert
            Assert.IsInstanceOf<List<Product>>(result);
            Assert.AreEqual(5, result.Count);
        }

    }
}