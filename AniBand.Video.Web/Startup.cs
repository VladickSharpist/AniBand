using AniBand.Video.Web.Extensions;
using AniBand.Web.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AniBand.Video.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddDatabase();
            services.AddServices();
            services.AddAuthentication(Configuration);
            services.AddWebVideoMapper();
            
            services.AddCors(options =>
                options.AddPolicy("Client", builder =>
                    builder
                        .AllowAnyHeader()
                        .WithOrigins(
                            Configuration.GetSection("Origins:3000").Value,
                            Configuration.GetSection("Origins:3001").Value,
                            Configuration.GetSection("Origins:3002").Value)
                        .AllowAnyMethod()
                        .AllowCredentials()));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors("Client");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHubs();
            });
        }
    }
}