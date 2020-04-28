using System;
using System.Data;
using System.Net.Http;
using HCrawler.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HCrawler.IntegrationTest
{
    public class WebFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly IServiceProvider _services;
        private readonly IDbTransaction _transaction;

        protected readonly HttpClient Client;
        private IServiceScope _scope;

        public WebFixture()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseEnvironment("Test");
                    webBuilder.UseTestServer();
                    webBuilder.UseStartup<TestStartup>();
                });

            var host = builder.Start();
            Client = host.GetTestClient();
            _services = host.Services;

            var connection = GetService<IDbConnection>();
            _transaction = connection.BeginTransaction();
        }

        protected T GetService<T>() => (T)_services.GetService(typeof(T));

        public void Dispose()
        {
                _transaction.Rollback();
                _transaction.Dispose();
        }
    }
}
