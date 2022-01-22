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
    public class OrderRepositoryTest : BaseRepositoryTest
    {
        private readonly IOrderRepository _repository;

        private readonly IProductRepository _productRepository;

        public OrderRepositoryTest()
        {
            _repository = new OrderRepository(_context);
            _productRepository = new ProductRepository(_context);
        }

        [Test]
        async public Task ShouldGet()
        {
            var product = ProductSeeder.SeedOne();
            await _productRepository.Insert(product);

            var seeder = OrderSeeder.SeedOne(product.Inventories.First().Id);
            await _repository.Insert(seeder);
            var test = _repository.Get(c => c.Id == seeder.Id);
            Assert.IsNotNull(test);
        }

        [Test]
        async public Task ShouldGetAll()
        {
            var product = ProductSeeder.SeedOne();
            await _productRepository.Insert(product);

            await _repository.InsertMany(OrderSeeder.SeedMany(product.Inventories.First().Id, 5, 5));
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
            var product = ProductSeeder.SeedOne();
            await _productRepository.Insert(product);

            var testData = OrderSeeder.SeedOne(product.Inventories.First().Id);
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
            var product = ProductSeeder.SeedOne();
            await _productRepository.Insert(product);

            var testData = OrderSeeder.SeedOne(product.Inventories.First().Id);
            await _repository.Insert(testData);

            testData.Street = "Taiwain";
            Assert.DoesNotThrowAsync(() => _repository.Update(testData));
        }

        [Test]
        async public Task ShouldInsert()
        {
            var product = ProductSeeder.SeedOne();
            await _productRepository.Insert(product);

            var data = OrderSeeder.SeedOne(product.Inventories.First().Id);
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