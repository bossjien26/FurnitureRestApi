using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DbEntity;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Middlewares;
using ResApi.src.Models;
using ResApi.src.Models.Response;
using src.Services.IService;
using src.Services.Service;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserInfoService _repository;
        private DbContextEntity _context;

        private readonly AppSettings _appSettings;

        public LoginController(DbContextEntity context, AppSettings appsetting)
        {
            _repository = new UserInfoService(context);
            _context = context;
            _appSettings = appsetting;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult login(LoginInfo loginInfo)
        {
            return Ok(new RegistrationResponse()
            {
                Status = true,
                Data = generateJwtToken(loginInfo)
            });
        }

        private string generateJwtToken(LoginInfo loginInfo)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", loginInfo.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}