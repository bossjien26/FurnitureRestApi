using System.Threading.Tasks;
using NUnit.Framework;
using RestApi.Test.DatabaseSeeders;
using src.Repositories.IRepository;
using src.Repositories.Repository;

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
            var testData = UserSeeder.Seedone();
            await _repository.Insert(testData);
            var User = await _repository.GetById(x => x.Id == testData.Id);
            Assert.NotNull(User);
        }
    }
}