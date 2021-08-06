﻿using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Extensions;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Extensions;
using AniBand.Auth.Services.Helpers;
using AniBand.Auth.Services.Services;
using AniBand.Auth.Web.Filters.Permission;
using AniBand.DataAccess;
using AniBand.DataAccess.Extensions;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AniBand.Auth.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration conf)
            => services
                .AddIdentity()
                .AddIdentityConfiguration(conf)
                .AddScoped<ITokenService, TokenService>();

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            => services
                .AddMapper();
        
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration conf)
            => services
                .AddContext(new ConfigurationHelper(conf))
                .AddRepositories();

        private static IServiceCollection AddContext(this IServiceCollection services, IConfigurationHelper confHelper)
            => services
                .AddDbContext<AniBandDbContext>(x =>
                    x.UseSqlServer(
                        confHelper.ConnectionString()));

        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddUser()
                .AddAuth()
                .AddHelpers();
        
        private static IServiceCollection AddMapper(this IServiceCollection services)
            => services
                .AddWebMapper()
                .AddServiceMapper();

        private static IServiceCollection AddIdentity(this IServiceCollection services)
            => services
                .AddIdentity<User, IdentityRole<long>>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>()
                .AddEntityFrameworkStores<AniBandDbContext>()
                .Services;

        private static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddJwtConfiguration(new ConfigurationHelper(configuration))
                .AddFilters();
        
        private static IServiceCollection AddFilters(this IServiceCollection services)
            => services
                .AddHandlers();

        private static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfigurationHelper confHelper)
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
                                .SecretKey())),
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

        private static IServiceCollection AddWebMapper(this IServiceCollection services)
            => services
                .AddAutoMapper(Assembly.GetExecutingAssembly());

        private static IServiceCollection AddHandlers(this IServiceCollection services)
            => services
                .AddSingleton<IAuthorizationHandler, PermissionHandler>();
        
    }
}