using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
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

        private string GetArguments(string url, string fullPath)
        {
            if (Regex.IsMatch(url, Reddit.vredditRegex))
            {
                return $" -f \"bestvideo+bestaudio\" --merge-output-format mp4 --output {fullPath} {url}";
            }
            if (Regex.IsMatch(url, Reddit.youtubeRegex))
            {
                return $" -f \"bestvideo[ext=webm]+bestaudio[ext=m4a]\" --merge-output-format mp4 --output {fullPath} {url}";
            }
            if (Regex.IsMatch(url, Reddit.gfycatRegex))
            {
                return $" -f best  --output {fullPath} {url}";
            }
            if (Regex.IsMatch(url, Reddit.redgifsJpegRegex))
            {
                return $" -f best  --output {fullPath} {url}";
            }

            return $" -f best --output {fullPath} {url}";
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
            process.StartInfo.Arguments = GetArguments(download.Url, fullPath);
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
                    var errorMessage = process.StandardError.ReadToEnd();
                    process.Dispose();
                    throw new Exception($"Process failed: {errorMessage}");
                }

                process.Dispose();
            });
        }
    }
}
