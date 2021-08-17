using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.src.Controllers;
using RestApi.src.Models;
using RestApi.Test.Repositories;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest : BaseRepositoryTest
    {
        private readonly UserController _controller;
        public UserControllerTest()
        {
            _controller = new UserController(
                _context,
                new Mock<ILogger<UserController>>().Object,
                new Mock<AppSettings>().Object);
        }

        [Test]
        public void ShouldGenerateJwtToken()
        {
            //Arrange &  Act
            var authenticateRequest = new AuthenticateRequest(){Mail = "adf",Password = "fsd"};
            //Act
            var result = _controller.Authenticate(authenticateRequest);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}