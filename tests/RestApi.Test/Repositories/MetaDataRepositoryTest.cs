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
    public class MetaDataRepositoryTest : BaseRepositoryTest
    {
        private readonly IMetaDataRepository _repository;

        public MetaDataRepositoryTest() =>
        _repository = new MetaDataRepository(_context);

        [Test]
        async public Task ShouldGet()
        {
            var seeder = MetaDataSeeder.SeedOne();
            await _repository.Insert(seeder);
            var product = _repository.Get(c => c.Id == seeder.Id);
            Assert.IsNotNull(product);
        }

        [Test]
        async public Task ShouldGetAll()
        {
            await _repository.InsertMany(MetaDataSeeder.SeedMany(5, 5));
            var products = await _repository.GetAll().Take(5).ToListAsync();
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
            var testData = MetaDataSeeder.SeedOne();
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
            var testData = MetaDataSeeder.SeedOne();
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