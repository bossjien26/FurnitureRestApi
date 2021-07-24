using System;
using DbEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private string sessionKey = "session_key";

        public LoginController(DbContextEntity context)
        {
            _repository = new UserInfoService(context);
            _context = context;
        }

        [HttpPost]
        [Route("process")]
        public IActionResult Process(LoginInfo loginInfo)
        {

            if (!string.IsNullOrWhiteSpace(loginInfo.Mail) &&
             !string.IsNullOrWhiteSpace(loginInfo.Password))
            {
                HttpContext.Session.SetString(sessionKey, "Peter");
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Welcome"
                });
            }
            else
            {
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Invalid"
                });
            }
        }
    }
}