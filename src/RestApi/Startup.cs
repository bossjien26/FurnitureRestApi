using System;
using System.Linq;
using System.Text;
using DbEntity;
using Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middlewares.Authentication;
using StackExchange.Redis;

namespace RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var appSettings = _configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            services.AddSingleton(appSettings);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.SetIsOriginAllowed(_ => true);
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                        builder.AllowCredentials();
                    });
            });

            // services.AddScoped<RequestHelper>();
            services.AddSingleton<SmtpMailConfig>(appSettings.SmtpMailConfig);

            services.AddSingleton<MailHelper>();

            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
                appSettings.RedisSettings.CartRedisSetting.DefaultConnection));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            services.AddDistributedMemoryCache();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true; // consent required
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestApi", Version = "v1" });
            });

            services.AddDbContext<DbContextEntity>(options =>
                options.UseMySql(
                    appSettings.ConnectionStrings.DefaultConnection,
                    ServerVersion.AutoDetect(appSettings.ConnectionStrings.DefaultConnection)
                )
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );

            services.AddAuthentication(options =>
            {
                //Authentication middleware configuration
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                //Mainly jwt token parameter setting
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    //Token issuer
                    ValidIssuer = appSettings.JwtSettings.Issuer,
                    //To whom
                    ValidAudience = appSettings.JwtSettings.ValidAudience,
                    //To encrypt the key here, you need to reference Microsoft.IdentityModel.Tokens
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                    //Whether to verify the validity period of Token, use the current time to compare with NotBefore and Expires in Token Claims
                    ValidateLifetime = true,
                    //Allowed server time offset
                    ClockSkew = TimeSpan.Zero

                };
            });

            IdentityModelEventSource.ShowPII = true;

            services.AddScoped<DbContextEntity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var appSettings = _configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            app.UseRouting();

            app.UseCors();


            app.Use(async (context, next) =>
            {
                // Add Header
                context.Response.Headers[appSettings.HeaderSettings.Response.FirstOrDefault().Title] = appSettings.HeaderSettings.Response.FirstOrDefault().Content;

                // Call next middleware
                await next.Invoke();
            });


            // app.UseMiddleware<AuthenticationMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApi v1");
                        c.RoutePrefix = string.Empty;
                    }
                );
            }

            // app.UseCookiePolicy();

            // app.UseHttpsRedirection();
            app.UseMiddleware<AuthenticationMiddleware>();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
