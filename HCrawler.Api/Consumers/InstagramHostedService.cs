using System;
using System.Threading.Tasks;
using HCrawler.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HCrawler.Api.Consumers
{
    public class InstagramHostedService : BaseHostedService
    {
        public InstagramHostedService(IServiceProvider serviceProvider, IConfiguration configuration) : base(
            "instagram", serviceProvider, configuration)
        {
        }

        protected override async Task HandleMessage(string content)
        {
            using var scope = ServiceProvider.CreateScope();
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
                await downloader.downloadHttp(download);
            }
        }
    }
}
