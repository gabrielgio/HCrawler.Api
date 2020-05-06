using System.Data;
using System.Linq;
using System.Reflection;
using HCrawler.CoreF;
using HCrawler.DB.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace HCrawler.IntegrationTest
{
    public class TestStartup
    {
        private readonly IConfiguration _configuration;

        public TestStartup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews();

            services.AddScoped<Image.Image>();
            services.AddScoped<IImageRepository, ImageRepository>();

            services.AddSingleton<IDbConnection>(t =>
            {
                var dbConnection = new NpgsqlConnection(_configuration["ConnectionString"]);
                dbConnection.Open();
                return dbConnection;
            });

            services.AddMvc()
                .AddApplicationPart(Assembly.Load(new AssemblyName("HCrawler.Api")));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
