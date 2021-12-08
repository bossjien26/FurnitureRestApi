using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class InventorySpecificationServiceTest
    {
        private readonly Mock<InventorySpecification> _entityMock;

        private readonly Mock<IInventorySpecificationRepository> _repoMock;

        private readonly Mock<IProductSpecificationRepository> _productSpecificationRepository;

        public InventorySpecificationServiceTest()
        {
            _entityMock = new Mock<InventorySpecification>();
            _repoMock = new Mock<IInventorySpecificationRepository>();
            _productSpecificationRepository = new Mock<IProductSpecificationRepository>();
        }

        [Test]
        public void ShouldInsert()
        => Assert.DoesNotThrowAsync(()
                => new InventorySpecificationService(_repoMock.Object, _productSpecificationRepository.Object).Insert(_entityMock.Object));

        [Test]
        public async Task ShouldGetById()
        {
            _repoMock.Setup(r => r.Get(x => x.Id == 1)).Returns(Task.FromResult(_entityMock.Object));
            var result = await new InventorySpecificationService(_repoMock.Object, _productSpecificationRepository.Object).GetById(1);
            Assert.IsInstanceOf<InventorySpecification>(result);
        }

        [Test]
        public async Task ShouldCheckInventoryAndInventorySpecificationIsExist()
        {
            _repoMock.Setup(r => r.Get(x => x.InventoryId == 1 && x.SpecificationContentId == 1)).Returns(Task.FromResult(_entityMock.Object));
            var result = await new InventorySpecificationService(_repoMock.Object, _productSpecificationRepository.Object).CheckInventoryAndInventorySpecificationIsExist(1, 1);
            Assert.True(result);
        }

        [Test]
        public void ShouldGetInventory()
        {
            int[] specificationContentIds = { 1 };
            _repoMock.Setup(r => r.Get(x => x.ProductSpecificationId == 1 && x.ProductSpecification.ProductId == 1)).Returns(Task.FromResult(_entityMock.Object));

            var result = new InventorySpecificationService(_repoMock.Object, _productSpecificationRepository.Object).GetInventory(1, specificationContentIds).ToList().FirstOrDefault();
            Assert.IsInstanceOf<int>(result);
        }
    }
}