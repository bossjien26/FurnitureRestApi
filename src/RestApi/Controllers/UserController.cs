using DbEntity;
using Microsoft.AspNetCore.Mvc;
using Entities;
using System.Linq;
using src.Services.Service;
using src.Services.IService;
using RestApi.src.Models.Response;
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
using Enum;
using RestApi.Models.Requests;
using Services.IService;
using Services.Service;

namespace RestApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _repository;

        private readonly ILogger<UserController> _logger;

        private readonly AppSettings _appSettings;

        private readonly MailHelper _mailHelper;

        private readonly IUserDetailService _userDetailService;

        public UserController(DbContextEntity context, ILogger<UserController> logger,
         AppSettings appsetting, MailHelper mailHelper)
        {
            _repository = new UserService(context);
            _userDetailService = new UserDetailService(context);
            _logger = logger;
            _appSettings = appsetting;
            _mailHelper = mailHelper;
        }

        [Authorize(Role.SuperAdmin, Role.Customer, Role.Admin, Role.Staff)]
        [HttpGet]
        [Route("ShowUsers")]
        public IActionResult ShowUser()
        {
            return Ok(_repository.GetMany(1, 10).ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("InsertUser")]
        public async Task<IActionResult> InsertUser(User user)
        {
            if (user == null)
            {
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Not Find"
                });
            }
            await _repository.Insert(user);
            return Ok(user);
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(User user)
        {
            if (user == null || _repository.GetById(user.Id).Result == null)
            {
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Not Find"
                });
            }

            _repository.Update(user);
            return Ok(new RegistrationResponse()
            {
                Status = true,
                Data = "Update"
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(Registration registration)
        {
            if (!CheckRegisterMailIsUse(registration.Mail))
            {
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Mail is registration"
                });
            }

            await CreateUserDetail(registration, await CreateUser(registration));

            SendRegisterMail(registration);
            return Ok(new RegistrationResponse()
            {
                Status = true,
                Data = "Register Success"
            });
        }

        private void SendRegisterMail(Registration registration)
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

        private async Task CreateUserDetail(Registration registration, User user)
        {
            await _userDetailService.Insert(
                new UserDetail()
                {
                    UserId = user.Id,
                    City = registration.City,
                    Country = registration.Country,
                    Street = registration.Street
                }
            );
        }

        private async Task<User> CreateUser(Registration registration)
        {
            var user = new User()
            {
                Mail = registration.Mail,
                Password = registration.Password,
                Name = registration.Name
            };
            await _repository.Insert(user);
            return user;
        }

        private bool CheckRegisterMailIsUse(string mail)
        {
            return (_repository.SearchUserMail(mail) != null) ? true : false;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate(Authenticate authenticateRequest)
        {
            if (_repository.GetVerifyUser(authenticateRequest.Mail, authenticateRequest.Password) == null)
            {
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Not Find"
                });
            }
            return Ok(new RegistrationResponse()
            {
                Status = true,
                Data = generateJwtToken(authenticateRequest)
            });
        }

        /// <summary>
        /// generate token that is valid for 7 days
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        private string generateJwtToken(Authenticate loginInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
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