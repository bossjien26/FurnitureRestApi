using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbEntity;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Interface;
using StackExchange.Redis;

namespace Middlewares.Authentication
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        private IUserService _userService;

        public AuthenticationMiddleware(RequestDelegate next) => _next = next;

        //TODO:need refactor
        public async Task Invoke(HttpContext httpContext, DbContextEntity context,
         AppSettings appSettings, ILogger<AuthenticationMiddleware> logger, IConnectionMultiplexer redis)
        {
            _userService = new UserService(context, redis);
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrWhiteSpace(token))
            {
                await _next.Invoke(httpContext);
                return;
            }

            if (!CheckJwtTokenIsExpire(token))
            {
                httpContext.Items["httpContextUser"] = null;
                await _next.Invoke(httpContext);
                return;
            }
            else
            {
                httpContext = await attachUserToContext(httpContext, context, token, appSettings, logger);
                await _next.Invoke(httpContext);
                return;
            }
        }

        private async Task<HttpContext> attachUserToContext(HttpContext httpContext, DbContextEntity context, string token,
        AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            try
            {
                var jwtToken = GetVerifyTokenType(appSettings, token);
                // attach user to context on successful jwt validation
                httpContext.Items["httpContextUser"] = jwtToken;
                await _userService.UserExpireDateTime(token, DateTime.UtcNow.AddHours(1));
                return httpContext;
            }
            catch (Exception exception)
            {
                logger.LogDebug(exception, "jwt validate is error");
                return httpContext;
            }
        }

        private JwtToken GetVerifyTokenType(AppSettings appSettings, string token)
        {
            var jwtTokenValues = VerifyToken(appSettings, token).Claims.
                    Where(x => x.Type == "mail" || x.Type == "password" || x.Type == "id")
                    .Select(s => s.Value).ToList();
            return CheckVerifyTokenType(jwtTokenValues, token);
        }

        private JwtToken CheckVerifyTokenType(List<string> jwtTokenValues, string token)
        {
            if (jwtTokenValues.Count() != 3)
            {
                return new JwtToken();
            }

            return new JwtToken()
            {
                Id = jwtTokenValues[0],
                Mail = jwtTokenValues[1],
                Password = jwtTokenValues[2],
                Token = token
            };
        }

        private JwtSecurityToken VerifyToken(AppSettings appSettings, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(appSettings.JwtSettings.Secret)
                ),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

        private bool CheckJwtTokenIsExpire(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token);

            var expDate = jwtToken.ValidTo;
            return expDate > DateTime.UtcNow.AddHours(8) ? true : false;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}