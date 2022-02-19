using System;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using RestApi.src.Models;
using Services;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDetailController : ControllerBase
    {
        private readonly ILogger<UserDetailController> _logger;
        private readonly IUserDetailService _userDetailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDetailController(DbContextEntity context, ILogger<UserDetailController> logger,
          IHttpContextAccessor httpContextAccessor)
        {
            _userDetailService = new UserDetailService(context);

            _logger = logger;

            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Authorize()]
        [Route("")]
        public async Task<IActionResult> Create(CreateUserDetailRequest registration)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            await CreateUserDetail(registration, Convert.ToInt32(userJWT.Id));

            return Created("", new AutResultResponse() { Status = true, Data = "Success" });
        }

        [HttpGet]
        [Authorize()]
        [Route("")]
        public async Task<IActionResult> GetUserDetailInfo()
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var userDetail = await _userDetailService.GetUserInfo(Convert.ToInt32(userJWT.Id));
            return Ok(userDetail == null ? new UserDetail() : userDetail);
        }

        private async Task CreateUserDetail(CreateUserDetailRequest registration, int userId)
        {
            await _userDetailService.Insert(
                new UserDetail()
                {
                    UserId = userId,
                    City = registration.City,
                    Country = registration.Country,
                    Street = registration.Street
                }
            );
        }
    }
}