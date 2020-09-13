using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HCrawler.Core;
using Microsoft.AspNetCore.Hosting;

namespace HCrawler.Consumers
{
    // TODO: this class is just chaotic
    public class Downloader : IDownloader
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _environment;

        public Downloader(IHttpClientFactory httpClientFactory, IWebHostEnvironment environment)
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

        private Task ConvertToWebm(string input, string output)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = $" -i {input} -c:v libvpx-vp9 -crf 15 -b:v 0 -b:a 128k -c:a libopus {output}";
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


        private string GetExt(string url)
        {
            if (Regex.IsMatch(url, Reddit.vredditRegex))
            {
                return ".mp4";
            }

            return ".webm";
        }

        private string GetYoutubeDlArguments(string url, string fullPath)
        {
            if (Regex.IsMatch(url, Reddit.vredditRegex))
            {
                return $" -f \"bestvideo+bestaudio/bestvideo\" --merge-output-format mp4 --output {fullPath} {url}";
            }

            if (Regex.IsMatch(url, Reddit.youtubeRegex))
            {
                return
                    $" -f \"bestvideo+bestaudio\" --merge-output-format webm --output {fullPath} {url}";
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


        public async Task downloadProcess(Payloads.Download download)
        {
            if (CreatAndGetFullPath(download, out var fullPath))
                return;

            var ext = GetExt(download.Url);

            if (ext == ".webm")
            {
                await DownloadYoutubeDl(download, fullPath);
                return;
            }

            var path = $"/tmp/{Guid.NewGuid().ToString()}{ext}";
            await DownloadYoutubeDl(download, path);
            await ConvertToWebm(path, fullPath);
            File.Delete(path);
        }

        private async Task DownloadYoutubeDl(Payloads.Download download, string fullPath)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = "youtube-dl";
            process.StartInfo.Arguments = GetYoutubeDlArguments(download.Url, fullPath);
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;

            process.Start();
            // TODO: not the best approach research for a better solution.
            await Task.Run(() =>
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
