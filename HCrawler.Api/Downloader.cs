using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HCrawler.Core;
using Microsoft.AspNetCore.Hosting;

namespace HCrawler.Api
{
    public class Downloader : IDownloader
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHostingEnvironment _environment;

        public Downloader(IHttpClientFactory httpClientFactory, IHostingEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        public async Task download(Payloads.Download data)
        {
            var fullPath = Path.Join(_environment.WebRootPath, data.Path);
            if (File.Exists(fullPath))
                return;

            var directoryName = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryName);

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(data.Url);

            var bytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(fullPath, bytes);
        }
    }
}
