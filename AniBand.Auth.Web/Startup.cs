using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AniBand.Auth.Web.Extensions;
using AniBand.Web.Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace AniBand.Auth.Web
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
            services.AddServices();
            services.AddAuthentication(Configuration);
            services.AddDatabase();
            services.AddWebAuthMapper();

            services.AddCors(options =>
                options.AddPolicy("Client", builder =>
                    builder
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002")
                        .AllowAnyMethod()
                        .AllowCredentials()));
            
            services.AddHttpContextAccessor();
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
