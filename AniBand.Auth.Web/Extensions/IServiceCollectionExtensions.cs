using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Services;
using AniBand.DataAccess;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.DataAccess.Repositories;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AniBand.Auth.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void JwtConfiguration(this IServiceCollection services, IConfiguration conf)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "bearer";
                options.DefaultChallengeScheme = "bearer";
            })
            .AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, ValidateIssuer = false, ValidateIssuerSigningKey = true
                    , IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(conf.GetSection("JWTSettings:securityKey").Value))
                    , ValidateLifetime = true, ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration conf)
        {
            var connectionString = conf.GetValue<string>("connectionString");
            services.AddDbContext<AniBandDbContext>(
                x => x.UseSqlServer(connectionString));
            services.AddRepositories();
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<long>>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>()
                .AddEntityFrameworkStores<AniBandDbContext>();
        }

        public static void ConfigureIdentity(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddIdentity();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            
            services.JwtConfiguration(configuration);
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<UserManager<User>>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseReadWriteRepository<RefreshToken>,
                BaseReadWriteRepository<RefreshToken>>();
        }
    }
}