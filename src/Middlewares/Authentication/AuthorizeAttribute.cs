using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Enum;
using Microsoft.Extensions.DependencyInjection;
using DbEntity;
using Services.Interface;
using Services;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Middlewares.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    //, IAuthorizationFilter
    public class AuthorizeAttribute : Attribute
    {
        private IUserService _userService;

        private readonly IList<RoleEnum> _roles;

        public AuthorizeAttribute(params RoleEnum[] roles)
        {
            _roles = roles ?? new RoleEnum[] { };
        }

        public async Task OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated withp [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var httpContextUser = (JwtToken)context.HttpContext.Items["httpContextUser"];
            var token = context.HttpContext.Request.Headers["Authorization"];
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DbContextEntity>();
            var redis = context.HttpContext.RequestServices.GetRequiredService<IConnectionMultiplexer>();
            _userService = new UserService(dbContext, redis);
            var redisUserInfo = await _userService.GetRedisUserInfo(token);

            var user = await _userService.GetVerifyUser(httpContextUser.Mail, httpContextUser.Password);
            if (user == null || redisUserInfo.IsNullOrEmpty || (_roles.Any() && !_roles.Contains(user.Role)))
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}