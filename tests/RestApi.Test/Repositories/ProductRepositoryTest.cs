using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories.Interface;
using Repositories;
using RestApi.Test.DatabaseSeeders;

namespace RestApi.Test.Repositories
{
    public class ProductRepositoryTest : BaseRepositoryTest
    {
        private readonly IProductRepository _repository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IProductSpecificationRepository _productSpecificationRepository;

        private readonly ISpecificationContentRepository _specificationContentRepository;

        public ProductRepositoryTest()
        {
            _repository = new ProductRepository(_context);

            _inventoryRepository = new InventoryRepository(_context);

            _productSpecificationRepository = new ProductSpecificationRepository(_context);

            _specificationContentRepository = new SpecificationContentRepository(_context);
        }

        [Test]
        async public Task ShouldGet()
        {
            var seeder = ProductSeeder.SeedOne();
            await _repository.Insert(seeder);

            var productSpecification = _productSpecificationRepository.Get(x => x.ProductId == seeder.Id).Result;
            var specificationContent = SpecificationSeeder.SeedContentOnce(productSpecification.SpecificationId);
            await _specificationContentRepository.Insert(specificationContent);

            var inventorySeeder = ProductSeeder.SeedInventoryOne(seeder.Id, productSpecification.Id
            , specificationContent.Id);
            await _inventoryRepository.Insert(inventorySeeder);

            var product = _repository.Get(c => c.Id == seeder.Id);
            Assert.IsNotNull(product);
        }

        [Test]
        async public Task ShouldGetAll()
        {
            await _repository.InsertMany(ProductSeeder.SeedMany(5, 5));
            var products = _repository.GetAll().Take(5).ToList();
            Assert.IsNotNull(products);
            Assert.AreEqual(5, products.Count);
        }

        [Test]
        public void ShouldCreateError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Insert(null));
        }

        [Test]
        public async Task ShouldDelete()
        {
            var testData = ProductSeeder.SeedOne();
            await _repository.Insert(testData);

            Assert.DoesNotThrowAsync(() => _repository.Delete(testData));
            Assert.IsNull(await _repository.Get(x => x.Id == testData.Id));
        }

        [Test]
        public void ShouldDeleteError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Delete(null));
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var testData = ProductSeeder.SeedOne();
            await _repository.Insert(testData);

            testData.IsDelete = true;
            Assert.DoesNotThrowAsync(() => _repository.Update(testData));
        }

        [Test]
        public void ShouldInsert()
        {
            var data = ProductSeeder.SeedOne();
            Assert.DoesNotThrowAsync(() => _repository.Insert(data));
            Assert.AreNotEqual(0, data.Id);
        }

        [Test]
        public void ShouldUpdateError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Update(null));
        }
    }
}