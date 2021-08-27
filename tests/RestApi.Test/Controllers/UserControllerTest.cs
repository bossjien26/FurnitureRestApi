using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Repositories.IRepository;
using RestApi.Models.Requests;
using RestApi.src.Controllers;
using RestApi.Test.DatabaseSeeders;
using RestApi.Test.Repositories;
using Repositories.Repository;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest : BaseRepositoryTest
    {
        private readonly UserController _controller;

        private readonly IUserRepository _repository;

        public UserControllerTest()
        {
            _controller = new UserController(
                _context,
                new Mock<ILogger<UserController>>().Object,
                new Mock<AppSettings>().Object,
                new Mock<MailHelper>(new Mock<SmtpMailConfig>().Object,
                 new Mock<ILogger<MailHelper>>().Object).Object);
                _repository = new UserRepository(_context);
        }

        [Test]
        public void ShouldGenerateJwtToken()
        {
            //Arrange &  Act
            var authenticateRequest = new Authenticate() { Mail = "adf", Password = "fsd" };
            //Assert
            var result = _controller.Authenticate(authenticateRequest);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ShouldShowUser()
        {
            //Arrange & Act & Assert
            var result = _controller.ShowUser();
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ShouldUpdateUserIsNull()
        {
            //Arrange & Act & Assert
            var result = _controller.UpdateUser(null);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShouldUpdateUser()
        {
            //Arrange
            var testData = UserSeeder.SeedOne();
            await _repository.Insert(testData);
            testData.IsDelete = true;

            //Act
            var result = _controller.UpdateUser(testData);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShouldInsertUserIsNUll()
        {
            //Arrange & Act & Assert
            var result = await _controller.InsertUser(null);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShouldInsertUser()
        {
            //Arrange
            var testData = UserSeeder.SeedOne();

            //Act
            var result = await _controller.InsertUser(testData);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ShouldRegistration()
        {
            //Arrange
            var testData = new Registration(){
                Name = "Name",
                Mail = "Mail",
                Password = "Password"
            };

            //Act
            var result = await _controller.Registration(testData);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}