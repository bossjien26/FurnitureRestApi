using System;
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
using src.Entities;
using src.Services.Service;

namespace src.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next) => _next = next;

        //TODO:need refactor
        public async Task Invoke(HttpContext httpContext, DbContextEntity context,
         AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var path = httpContext.Request.Path;
            attachUserToContext(httpContext, context, token, appSettings, logger);
            await _next.Invoke(httpContext);
            return;
        }

        private void attachUserToContext(HttpContext httpContext, DbContextEntity context, string token,
        AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            var _repository = new UserInfoService(context);
            try
            {
                var userId = int.Parse(VerifyToken(appSettings, token).Claims.
                            First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                httpContext.Items["User"] = _repository.FindUser(userId).Result;
            }
            catch (Exception exception)
            {
                logger.LogDebug(exception, "jwt validate is error");
            }
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