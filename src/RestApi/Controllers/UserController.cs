using DbEntity;
using Microsoft.AspNetCore.Mvc;
using Entities;
using System.Linq;
using Services;
using Services.Interface;
using Microsoft.Extensions.Logging;
using Middlewares;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using Helpers;
using System.Threading.Tasks;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using Enum;
using Microsoft.AspNetCore.Http;
using RestApi.src.Models;
using RestApi.Models.Response;
using StackExchange.Redis;

namespace RestApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        private readonly ILogger<UserController> _logger;

        private readonly AppSettings _appSettings;

        private readonly MailHelper _mailHelper;

        private readonly IUserDetailService _userDetailService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(DbContextEntity context, ILogger<UserController> logger,
         AppSettings appsetting, MailHelper mailHelper, IHttpContextAccessor httpContextAccessor, IConnectionMultiplexer redis)
        {
            _service = new UserService(context, redis);
            _userDetailService = new UserDetailService(context);
            _logger = logger;
            _appSettings = appsetting;
            _mailHelper = mailHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpGet]
        [Route("{perPage}")]
        public IActionResult ShowUser(int perPage)
        => Ok(_service.GetMany(perPage, 10).ToList());

        /// <summary>
        /// identity user token verification
        /// </summary>
        /// <param name="CheckIdentityVerificationRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize()]
        [Route("identity/verification")]
        public IActionResult CheckIdentityVerification()
        {
            return NoContent();
        }

        [HttpPut]
        [Authorize()]
        [Route("")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var user = await _service.GetById(Convert.ToInt32(userJWT.Id));
            user.Name = request.Name;

            await _service.Update(user);
            return Ok(_service.MapShowUserInfo(user));
        }

        [HttpGet]
        [Authorize()]
        [Route("info")]
        public async Task<IActionResult> GetUserInfo(string token)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var user = await _service.GetById(Convert.ToInt32(userJWT.Id));
            return Ok(_service.MapShowUserInfo(user));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(RegistrationRequest registration)
        {
            if (CheckRegisterMailIsUse(registration.Mail))
            {
                return NotFound(new AutResultResponse()
                {
                    Status = false,
                    Data = "Mail is registration"
                });
            }

            await CreateUser(registration);

            SendRegisterMail(registration);
            return Created("", new AutResultResponse()
            {
                Status = true,
                Data = "Register Success"
            });
        }

        private void SendRegisterMail(RegistrationRequest registration)
        {
            try
            {
                _mailHelper.SendMail(new Mailer()
                {
                    MailTo = registration.Mail,
                    NameTo = registration.Name,
                    Subject = "test",
                    Content = "verify mail"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private async Task<User> CreateUser(RegistrationRequest registration)
        {
            var user = new User()
            {
                Mail = registration.Mail,
                Password = registration.Password,
                Name = registration.Name
            };
            await _service.Insert(user);
            return user;
        }

        private bool CheckRegisterMailIsUse(string mail)
        {
            return (_service.SearchUserMail(mail) != null) ? true : false;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Authenticate(GenerateAuthenticateRequest authenticateRequest)
        {
            var user = await _service.GetVerifyUser(authenticateRequest.Mail, authenticateRequest.Password);
            if (user == null)
            {
                return NotFound(new AutResultResponse()
                {
                    Status = false,
                    Data = "Not Find"
                });
            }
            var token = generateJwtToken(user.Id, authenticateRequest);
            await _service.Login(token, user.Id.ToString());
            await _service.UserExpireDateTime(token, DateTime.UtcNow.AddHours(1));
            return Ok(new AuthenticateResponse()
            {
                Token = token
            });
        }

        [Authorize()]
        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            _service.Logout(userJWT.Token);
            return NoContent();
        }

        /// <summary>
        /// generate token that is valid for 7 days
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private string generateJwtToken(int userId, GenerateAuthenticateRequest loginInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", userId.ToString()),
                    new Claim("mail", loginInfo.Mail.ToString()),
                    new Claim("password", loginInfo.Password.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}