using System.Threading.Tasks;
using Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestApi.Models.Requests;
using RestApi.src.Controllers;
using RestApi.Test.DatabaseSeeders;
using System.Net;
using Services;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest : BaseController
    {
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _controller = new UserController(
                _context,
                new Mock<ILogger<UserController>>().Object,
                new Mock<AppSettings>().Object,
                new Mock<MailHelper>(new Mock<SmtpMailConfig>().Object,
                new Mock<ILogger<MailHelper>>().Object).Object,
                new Mock<IHttpContextAccessor>().Object
            );
        }

        [Test]
        public void ShouldGenerateJwtToken()
        {
            //Arrange &  Act
            var request = new Authenticate() { Mail = "jan@example.com", Password = "aaaaaaa" };
            var response = _httpClient.PostAsync("/api/user/authenticate", PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }

        [Test]
        public void ShouldShowMany()
        {
            //Arrange & Act & Assert
            var response = _httpClient.GetAsync("/api/user/1");

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
        }

        [Test]
        public async Task ShouldUpdateUser()
        {
            var service = new UserService(_context);
            var user = service.GetVerifyUser("jan@example.com", "aaaaaaa");

            user.Name = "test";
            var response = await _httpClient.PutAsync("/api/user", PostType(user));

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task ShouldRegistration()
        {
            //Arrange
            var user = UserSeeder.SeedOne();
            var request = new Registration()
            {
                Name = user.Name,
                Mail = user.Mail,
                Password = user.Password
            };

            var response = await _httpClient.PostAsync("/api/user/registration",
            PostType(request));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }
    }
}