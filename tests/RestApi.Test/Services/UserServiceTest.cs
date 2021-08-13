using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using RestApi.Test.DatabaseSeeders;
using src.Repositories.IRepository;
using src.Services.Service;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class UserServiceTest
    {
        private readonly Mock<User> _entityMock;

        private readonly Mock<IUserRepository> _repoMock;

        public UserServiceTest()
        {
            _entityMock = new Mock<User>();

            _repoMock = new Mock<IUserRepository>();
        }

        [Test]
        async public Task ShouldGetById()
        {
            // Arrange
            _repoMock.Setup(r => r.GetById(x => x.Id == 1))
                .Returns(Task.FromResult(_entityMock.Object));

            // Act
            var result = await new UserService(_repoMock.Object).GetById(1);

            // Assert
            Assert.IsInstanceOf<User>(result);
        }

        [Test]
        public void ShouldGetMany()
        {
            //Arrange
            var users = UserSeeder.SeedMany(10,15).AsQueryable();

            _repoMock.Setup(u => u.GetAll()).Returns(users);

            //Atc
            var result = new UserService(_repoMock.Object).GetMany(1,5).ToList();

            //Assert
            Assert.IsInstanceOf<List<User>>(result);
            Assert.AreEqual(5,result.Count);
        }

        [Test]
        public void ShouldInsertOne()
        {
            //Arrange
            //Act
            //Assert
            Assert.DoesNotThrowAsync(() =>
                new UserService(_repoMock.Object).Insert(_entityMock.Object)
            );
        }

        [Test]
        public void ShouldUpdate()
        {
            //Arrange
            //Act
            //Asset
            _repoMock.Setup(c => c.GetById(x => x.Name == "jan"))
                .Returns(Task.FromResult(_entityMock.Object));
            
            _entityMock.Object.Name = "test";

            Assert.DoesNotThrow(() => 
                new UserService(_repoMock.Object).Update(_entityMock.Object)
            );
        }

        [Test]
        public void ShouldGetVerifyUser()
        {
            var mail = "example@mail.com";
            var password = "abc";
            _repoMock.Setup(c => c.GetById(x => x.Mail == mail
                && x.Password == password)).Returns(Task.FromResult(_entityMock.Object));

            Assert.DoesNotThrow(() =>
                new UserService(_repoMock.Object).GetVerifyUser(mail,password)
            );
        }
    }
}