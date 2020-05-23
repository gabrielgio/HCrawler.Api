using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HCrawler.Api.Consumers
{
    public abstract class BaseHostedService : BackgroundService
    {
        protected readonly IServiceProvider ServiceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _channelName;

        public BaseHostedService(string channelName, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _channelName = channelName;
            ServiceProvider = serviceProvider;
            var factory = new ConnectionFactory {HostName = configuration["rabbit"]};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_channelName, false, false, false, null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += ConsumerOnReceived;

            _channel.BasicConsume(_channelName, false, consumer);
        }

        private async void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            var content = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

            await HandleMessage(content);

            _channel.BasicAck(ea.DeliveryTag, false);
        }

        protected abstract Task HandleMessage(string content);

        public override void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
            base.Dispose();
        }

      
    }
}
