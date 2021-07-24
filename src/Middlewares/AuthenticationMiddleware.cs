using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace src.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path.Value == "/api/login/process")
            {
                await _next.Invoke(httpContext);
                return;
            }else{
                if (httpContext.Session.GetString("username") == null)
                {
                    httpContext.Response.StatusCode = 400; //Bad Request                
                    await httpContext.Response.WriteAsync("Please Login");
                    return;
                }
            }
            await _next.Invoke(httpContext);
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