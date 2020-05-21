using System;
using System.Threading;
using System.Threading.Tasks;
using HCrawler.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HCrawler.Api
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public ConsumeRabbitMQHostedService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory {HostName = configuration["rabbit"]};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("instagram", false, false, false, null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += ConsumerOnReceived;

            _channel.BasicConsume("instagram", false, consumer);
        }

        private async void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

            await HandleMessage(content);

            _channel.BasicAck(ea.DeliveryTag, false);
        }

        public override void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
            base.Dispose();
        }

        private async Task HandleMessage(string content)
        {
            using var scope = _serviceProvider.CreateScope();
            var image = scope.ServiceProvider.GetRequiredService<Image.Image>();
            var downloader = scope.ServiceProvider.GetRequiredService<IDownloader>();

            var post = Instagram.parsePost(content);

            var createImages = Instagram.getPayload(post);
            foreach (var createImage in createImages)
            {
                await image.createImageIfNotExistsAsync(createImage);
            }

            var downloadPost = Instagram.getDownloadPost(post);
            foreach (var download in downloadPost)
            {
                await downloader.download(download);
            }
        }
    }
}
