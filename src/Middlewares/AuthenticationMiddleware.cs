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
            if (string.IsNullOrWhiteSpace(token))
            {
                await _next.Invoke(httpContext);
                return;
            }
            var path = httpContext.Request.Path;
            attachUserToContext(httpContext, context, token, appSettings, logger);
            await _next.Invoke(httpContext);
            return;
        }

        private void attachUserToContext(HttpContext httpContext, DbContextEntity context, string token,
        AppSettings appSettings, ILogger<AuthenticationMiddleware> logger)
        {
            try
            {
                var jwtToken = GetVerifyTokenType(appSettings, token);
                // attach user to context on successful jwt validation
                httpContext.Items["User"] = new UserInfoService(context).
                GetVerifyUser(jwtToken.Mail, jwtToken.Password);
            }
            catch (Exception exception)
            {
                logger.LogDebug(exception, "jwt validate is error");
            }
        }

        private JwtToken GetVerifyTokenType(AppSettings appSettings, string token)
        {
            var jwtTokenValues = VerifyToken(appSettings, token).Claims.
                    Where(x => x.Type == "mail" || x.Type == "password").Select(s => s.Value).ToList();
            return CheckVerifyTokenType(jwtTokenValues);
        }

        private JwtToken CheckVerifyTokenType(List<string> jwtTokenValues)
        {
            if (jwtTokenValues.Count() != 2)
            {
                return new JwtToken();
            }

            return new JwtToken()
            {
                Mail = jwtTokenValues[0],
                Password = jwtTokenValues[1]
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
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }

    public class JwtToken
    {
        public string Mail { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}