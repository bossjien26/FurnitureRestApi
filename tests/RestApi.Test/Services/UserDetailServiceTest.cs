using System.Threading.Tasks;
using Entities;
using Moq;
using NUnit.Framework;
using Repositories.Interface;
using Services;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class UserDetailServiceTest
    {
        private readonly Mock<UserDetail> _entityMock;

        private readonly Mock<IUserDetailRepository> _repoMock;

        private readonly Mock<User> _entityUserMock;

        private readonly Mock<IUserRepository> _repoUserMock;

        public UserDetailServiceTest()
        {
            _entityMock = new Mock<UserDetail>();

            _repoMock = new Mock<IUserDetailRepository>();

            _entityUserMock = new Mock<User>();

            _repoUserMock = new Mock<IUserRepository>();
        }

        [Test]
        public async Task ShouldInsertOne()
        {
            //Arrange
            var userService = new UserService(_repoUserMock.Object);

            //Act
            await userService.Insert(_entityUserMock.Object);
            _entityMock.Object.UserId = _entityUserMock.Object.Id;

            //Assert
            Assert.DoesNotThrowAsync(() =>
                new UserDetailService(_repoMock.Object).Insert(_entityMock.Object)
            );
        }
    }
}