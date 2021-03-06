using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;

namespace HCrawler.DB
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServices();

            using var scope = serviceProvider.CreateScope();
            
            UpdateDatabase(scope.ServiceProvider);
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .AddScoped<IConfiguration>(x =>
                {
                    var env = new EnvironmentVariablesConfigurationProvider();
                    return new ConfigurationRoot(new[] {env});
                })
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(z =>
                    {
                        var configuration = z.GetService<IConfiguration>();
                        return configuration["ConnectionString"];
                    })
                    .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            serviceProvider
                .GetRequiredService<IMigrationRunner>()
                .MigrateUp();
        }
    }
}
