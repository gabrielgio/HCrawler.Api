using System;
using System.Threading.Tasks;
using HCrawler.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HCrawler.Api.Consumers
{
    public class RedditHostedService : BaseHostedService
    {
        private readonly ILogger<RedditHostedService> _logger;

        public RedditHostedService(IServiceProvider serviceProvider, IConfiguration configuration,
            ILogger<RedditHostedService> logger) : base("reddit", serviceProvider, configuration)
        {
            _logger = logger;
        }

        protected override async Task HandleMessage(string content)
        {
            try
            {
                var post = Reddit.parsePost(content);

                var urlMethodType = Reddit.isKnown(post);
                if (urlMethodType == Reddit.UrlMethodType.Unknown)
                    return;
                
                using var scope = ServiceProvider.CreateScope();
                var image = scope.ServiceProvider.GetRequiredService<Image.Image>();
                var downloader = scope.ServiceProvider.GetRequiredService<IDownloader>();

                var createImage = Reddit.getPayload(post);
                var download = Reddit.getDownloadPost(post);

                await image.createImageIfNotExistsAsync(createImage);

                if (urlMethodType == Reddit.UrlMethodType.Http)
                    await downloader.downloadHttp(download);
                else
                    await downloader.downloadProcess(download);
            }
            catch (Exception e)
            {
                //TODO: REMOVE THIS IN THE FUTURE
                //I'll leave this for now to get which payloads are throwing errors
                _logger.LogError(e, content);
            }
        }
    }
}
