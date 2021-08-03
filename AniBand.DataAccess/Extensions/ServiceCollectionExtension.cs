using System;
using System.Text;
using System.Threading.Tasks;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.DataAccess.Repositories;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AniBand.DataAccess.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddIdentity(this IServiceCollection services)
            => services.AddIdentity<User, IdentityRole<long>>()
                .AddRoleManager<RoleManager<IdentityRole<long>>>()
                .AddEntityFrameworkStores<AniBandDbContext>();

        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services.AddScoped<IBaseReadWriteRepository<RefreshToken>,
                    BaseReadWriteRepository<RefreshToken>>()
                .AddScoped<IBaseReadonlyRepository<UserToken>,
                    BaseReadonlyRepository<UserToken>>();
        
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .JwtConfiguration(configuration);
        }
        
        public static void JwtConfiguration(this IServiceCollection services, IConfiguration conf)
        {
            services.AddAuthentication(options =>
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
                            .GetBytes(conf
                                .GetSection("JWTSettings:securityKey")
                                .Value)),
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
                });
        }
    }
}