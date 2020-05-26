using System;
using System.Diagnostics;
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

        private bool CreatAndGetFullPath(Payloads.Download data, out string fullPath)
        {
            fullPath = Path.Join(_environment.WebRootPath, data.Path);
            if (File.Exists(fullPath))
                return true;

            var directoryName = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directoryName);
            return false;
        }

        public async Task downloadHttp(Payloads.Download download)
        {
            if (CreatAndGetFullPath(download, out var fullPath))
                return;

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync(download.Url);

            var bytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(fullPath, bytes);
        }


        public Task downloadProcess(Payloads.Download download)
        {
            if (CreatAndGetFullPath(download, out var fullPath))
                return Task.CompletedTask;

            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "youtube-dl";
            process.StartInfo.Arguments =
                $" -f best --merge-output-format webm --output {fullPath} {download.Url}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;

            process.Start();
            // TODO: not the best approach research for a better solution.
            return Task.Run(() =>
            {
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    process.Dispose();
                    throw new Exception("Process failed");
                }

                process.Dispose();
            });
        }
    }
}
