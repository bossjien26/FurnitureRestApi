using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories;
using Repositories.Interface;
using RestApi.Test.DatabaseSeeders;

namespace RestApi.Test.Repositories
{
    public class MetadataRepositoryTest : BaseRepositoryTest
    {
        private readonly IMetadataRepository _repository;

        public MetadataRepositoryTest() =>
        _repository = new MetadataRepository(_context);

        [Test]
        async public Task ShouldGet()
        {
            var seeder = MetadataSeeder.SeedOne();
            await _repository.Insert(seeder);
            var product = _repository.Get(c => c.Id == seeder.Id);
            Assert.IsNotNull(product);
        }

        [Test]
        async public Task ShouldGetAll()
        {
            await _repository.InsertMany(MetadataSeeder.SeedMany(5, 5));
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
            var testData = MetadataSeeder.SeedOne();
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
            var testData = MetadataSeeder.SeedOne();
            await _repository.Insert(testData);

            testData.Value = "Test";
            Assert.DoesNotThrowAsync(() => _repository.Update(testData));
        }

        [Test]
        public void ShouldUpdateError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Update(null));
        }
    }
}