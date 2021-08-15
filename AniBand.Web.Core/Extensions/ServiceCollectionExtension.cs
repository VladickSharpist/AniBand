using System;
using System.Text;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Extensions;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Extensions;
using AniBand.Auth.Services.Services;
using AniBand.Auth.Web.Filters.Permission;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Extensions;
using AniBand.DataAccess;
using AniBand.DataAccess.Extensions;
using AniBand.Domain.Models;
using AniBand.Video.Services.Abstractions.Extensions;
using AniBand.Video.Services.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AniBand.Web.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthentication(
            this IServiceCollection services, 
            IConfiguration conf)
            => services
                .AddIdentity()
                .AddIdentityConfiguration()
                .AddScoped<ITokenService, TokenService>();

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration conf)
            => services
                .AddMapper()
                .AddHelpers(conf)
                .AddLoggers()
                .StorageConfiguration();
        
        public static IServiceCollection AddDatabase(this IServiceCollection services)
            => services
                .AddContext(services
                    .BuildServiceProvider()
                    .GetRequiredService<IConfigurationHelper>())
                .AddRepositories();
        
        private static IServiceCollection AddContext(
            this IServiceCollection services,
            IConfigurationHelper confHelper)
            => services
                .AddDbContext<AniBandDbContext>(x =>
                    x.UseSqlServer(
                        confHelper.ConnectionString),ServiceLifetime.Transient);

        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddUser()
                .AddAuth()
                .AddVideo();
        
        private static IServiceCollection AddMapper(this IServiceCollection services)
            => services
                .AddAuthServiceMapper()
                .AddVideoServiceMapper();

        private static IServiceCollection AddIdentity(this IServiceCollection services)
            => services
                .AddIdentity<User, IdentityRole<long>>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>()
                .AddEntityFrameworkStores<AniBandDbContext>()
                .Services;

        private static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
            => services
                .Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddJwtConfiguration(services
                    .BuildServiceProvider()
                    .GetRequiredService<IConfigurationHelper>())
                .AddFilters();
        
        private static IServiceCollection AddFilters(this IServiceCollection services)
            => services
                .AddHandlers();

        private static IServiceCollection AddJwtConfiguration(
            this IServiceCollection services,
            IConfigurationHelper confHelper)
            => services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false, 
                        ValidateIssuer = false, 
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(confHelper
                                .SecretKey)),
                        ValidateLifetime = true, 
                        ClockSkew = TimeSpan.Zero
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
                })
                .Services;
        
        private static IServiceCollection AddHandlers(this IServiceCollection services)
            => services
                .AddSingleton<IAuthorizationHandler, PermissionHandler>();
    }
}