using System.Threading.Tasks;
using NUnit.Framework;
using RestApi.Models.Requests;
using RestApi.Test.DatabaseSeeders;
using System.Net;
using Services;
using Services.Interface;

namespace RestApi.Test.Controllers
{
    [TestFixture]
    public class UserControllerTest : BaseController
    {
        private readonly IUserService _userService;

        public UserControllerTest()
        {
            _userService = new UserService(_context, _redisConnect);
        }

        [Test]
        public void ShouldGenerateJwtToken()
        {
            //Arrange &  Act
            var request = new GenerateAuthenticateRequest() { Mail = "jan@example.com", Password = "aaaaaaa" };
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
            var user = await _userService.GetVerifyUser("jan@example.com", "aaaaaaa");

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
            var request = new RegistrationRequest()
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