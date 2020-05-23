using System;
using System.Threading.Tasks;
using HCrawler.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HCrawler.Api.Consumers
{
    public class RedditHostedService : BaseHostedService
    {
        public RedditHostedService(IServiceProvider serviceProvider, IConfiguration configuration) : base(
            "reddit", serviceProvider, configuration)
        {
        }

        protected override async Task HandleMessage(string content)
        {
            var post = Reddit.parsePost(content);

            if (Reddit.isKnown(post))
            {
                using var scope = ServiceProvider.CreateScope();
                var image = scope.ServiceProvider.GetRequiredService<Image.Image>();
                var downloader = scope.ServiceProvider.GetRequiredService<IDownloader>();


                var createImage = Reddit.getPayload(post);
                await image.createImageIfNotExistsAsync(createImage);

                var download = Reddit.getDownloadPost(post);
                await downloader.download(download);
            }
        }
    }
}
