using DbEntity;
using Microsoft.AspNetCore.Mvc;
using Entities;
using System.Linq;
using src.Services.Service;
using src.Services.IService;
using ResApi.src.Models.Response;
using Microsoft.Extensions.Logging;
using Middlewares;
using ResApi.src.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System;
using Helpers;

namespace ResApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoService _repository;

        private readonly ILogger<UserController> _logger;

        private readonly AppSettings _appSettings;

        public UserController(DbContextEntity context, ILogger<UserController> logger,
         AppSettings appsetting)
        {
            _repository = new UserInfoService(context);
            _logger = logger;
            _appSettings = appsetting;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_repository.GetAllUser().ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("InsertUser")]
        public IActionResult InsertUser()
        {
            var user = new User()
            {
                Mail = "bbbbb"
            };
            _repository.Insert(user);
            return Ok(user);
        }

        [HttpPut]
        [Authorize]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(User user)
        {
            if (_repository.FindUser(user.Id).Result == null)
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
        [Route("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest authenticateRequest)
        {
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
        private string generateJwtToken(AuthenticateRequest loginInfo)
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