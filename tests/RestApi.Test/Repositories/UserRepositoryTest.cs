using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories.Interface;
using RestApi.Test.DatabaseSeeders;
using Repositories;

namespace RestApi.Test.Repositories
{
    [TestFixture]
    public class UserRepositoryTest : BaseRepositoryTest
    {
        private readonly IUserRepository _repository;

        public UserRepositoryTest() => _repository =
            new UserRepository(_context);

        [Test]
        async public Task ShouldGet()
        {
            var testData = UserSeeder.SeedOne();
            await _repository.Insert(testData);
            var User = await _repository.Get(x => x.Id == testData.Id);
            Assert.NotNull(User);
        }

        [Test]
        async public Task ShouldGetAll()
        {
            await _repository.InsertMany(UserSeeder.SeedMany(5, 5));
            var users =  _repository.GetAll().Take(5).ToList();
            Assert.NotNull(users);
            Assert.AreEqual(5, users.Count);
        }

        [Test]
        public void ShouldCreateError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Insert(null));
        }

        [Test]
        public void ShouldInsert()
        {
            var data = UserSeeder.SeedOne();
            Assert.DoesNotThrowAsync(() => _repository.Insert(data));
            Assert.AreNotEqual(0, data.Id);
        }

        [Test]
        public void ShouldDeleteError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Delete(null));
        }

        [Test]
        public async Task ShouldDelete()
        {
            var testData = UserSeeder.SeedOne();
            await _repository.Insert(testData);

            Assert.DoesNotThrowAsync(() => _repository.Delete(testData));
            Assert.IsNull(await _repository.Get(x => x.Id == testData.Id));
        }

        [Test]
        public void ShouldUpdateError()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _repository.Update(null));
        }

        [Test]
        public async Task ShouldUpdate()
        {
            var testData = UserSeeder.SeedOne();
            await _repository.Insert(testData);

            testData.IsDelete = true;
            Assert.DoesNotThrowAsync(() => _repository.Update(testData));
        }
    }
}