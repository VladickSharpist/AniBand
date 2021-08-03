using System.Reflection;
using AniBand.Auth.Services.Extensions;
using AniBand.DataAccess;
using AniBand.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Auth.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMapper(this IServiceCollection services)
            => services.AddAutoMapper(Assembly.GetExecutingAssembly());

        public static void AddDatabase(this IServiceCollection services, IConfiguration conf)
            => services.AddDbContext<AniBandDbContext>(x =>
                    x.UseSqlServer(
                        conf.GetValue<string>("connectionString")))
                .AddRepositories()
                .ConfigureIdentity(conf);

        public static IServiceCollection AddServices(this IServiceCollection services)
            => services.AddUser()
                .AddAuth();
    }
}