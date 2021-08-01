using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbEntity;
using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
        public async Task Invoke(HttpContext httpContext, DbContextEntity context,AppSettings appSettings)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var path = httpContext.Request.Path;

            if (path.Value == "/api/login/login")
            {
                await _next.Invoke(httpContext);
                return;
            }
            else
            {
                attachUserToContext(httpContext, context, token,appSettings);
                var user = (User)httpContext.Items["User"];
                if (user == null)
                {
                    httpContext.Response.StatusCode = 400;
                    await httpContext.Response.WriteAsync("Please Login");
                    return;
                }
                await _next.Invoke(httpContext);
                return;
            }
        }

        // private async Task CheckLoginSession(HttpContext httpContext)
        // {
        //     var path = httpContext.Request.Path;
        //     if (path.Value == "/api/login/login")
        //     {
        //         await _next.Invoke(httpContext);
        //         return;
        //     }
        //     else
        //     {
        //         if (httpContext.Session.GetString("session_key") == null)
        //         {
        //             httpContext.Response.StatusCode = 400; //Bad Request                
        //             await httpContext.Response.WriteAsync("Please Login");
        //             return;
        //         }
        //     }
        // }

        private void attachUserToContext(HttpContext httpContext, DbContextEntity context, string token,AppSettings appSettings)
        {
            var _repository = new UserInfoService(context);
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings.JwtSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                httpContext.Items["User"] = _repository.FindUser(userId).Result;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
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