using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HCrawler.Api.ViewModels;
using HCrawler.Core;
using Newtonsoft.Json;
using Xunit;

namespace HCrawler.IntegrationTest
{
    public class ImageTest : WebFixture<TestStartup>
    {
        private readonly CreateImage _createImage;

        public ImageTest()
        {
            _createImage = new CreateImage
            {
                CreatedOn = DateTime.UtcNow,
                ImagePath = "/root/Pictures",
                ImageUrl = "https://duckduckgo.com",
                ProfileName = "SomeDude",
                ProfileUrl = "http://duckduckgo.com",
                SourceName = "SomePace",
                SourceUrl = "https://someplece.com/"
            };
            Init().Wait();
        }

        private async Task Init()
        {
            var image = GetService<Image.Image>();
            await image.createImageIfNotExistsAsync(_createImage.ToRecord());

            var oldCreateImage = new CreateImage
            {
                CreatedOn = DateTime.UtcNow.AddDays(-1),
                ImagePath = "/root/Pictures2",
                ImageUrl = "https://duckduckgo2.com",
                ProfileName = "SomeDude",
                ProfileUrl = "http://duckduckgo.com",
                SourceName = "SomePace",
                SourceUrl = "https://someplece.com/"
            };
            await image.createImageIfNotExistsAsync(oldCreateImage.ToRecord());

            oldCreateImage = new CreateImage
            {
                CreatedOn = DateTime.UtcNow.AddDays(-1),
                ImagePath = "/root/Pictures3",
                ImageUrl = "https://duckduckgo3.com",
                ProfileName = "SomeDude2",
                ProfileUrl = "http://duckduckgo2.com",
                SourceName = "SomePace",
                SourceUrl = "https://someplece.com/"
            };
            await image.createImageIfNotExistsAsync(oldCreateImage.ToRecord());
        }

        [Fact]
        public async Task GetAsync()
        {
            const string route = "/images";
            var response = await Client.GetAsync(route);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var images = JsonConvert.DeserializeObject<IEnumerable<Proxies.DetailedImage>>(json).ToList();
            var image = images.First();

            Assert.True(3 == images.Count);

            Assert.Equal(_createImage.ImagePath, image.Path);
            Assert.Equal(_createImage.ImageUrl, image.Url);
            Assert.Equal(_createImage.ProfileName, image.DetailedProfile.Name);
            Assert.Equal(_createImage.ProfileUrl, image.DetailedProfile.Url);
            Assert.Equal(_createImage.SourceName, image.DetailedProfile.DetailedSource.Name);
            Assert.Equal(_createImage.SourceUrl, image.DetailedProfile.DetailedSource.Url);
        }


        [Fact]
        public async Task GetAsyncWithParam()
        {
            var route = $"/images?name={_createImage.ProfileName}";
            var response = await Client.GetAsync(route);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var images = JsonConvert.DeserializeObject<IEnumerable<Proxies.DetailedImage>>(json).ToList();
            var image = images.First();

            Assert.True(2 == images.Count);

            Assert.Equal(_createImage.ImagePath, image.Path);
            Assert.Equal(_createImage.ImageUrl, image.Url);
            Assert.Equal(_createImage.ProfileName, image.DetailedProfile.Name);
            Assert.Equal(_createImage.ProfileUrl, image.DetailedProfile.Url);
            Assert.Equal(_createImage.SourceName, image.DetailedProfile.DetailedSource.Name);
            Assert.Equal(_createImage.SourceUrl, image.DetailedProfile.DetailedSource.Url);
        }

        [Fact]
        public async Task GetAsyncWithParam_Empty()
        {
            const string route = "/images?name=skjdskdskja";
            var response = await Client.GetAsync(route);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var json = await response.Content.ReadAsStringAsync();
            var images = JsonConvert.DeserializeObject<IEnumerable<Proxies.DetailedImage>>(json).ToList();

            Assert.True(0 == images.Count);
        }


        [Fact]
        public async Task CreateImage()
        {
            const string route = "/images";
            var json = JsonConvert.SerializeObject(_createImage);
            var stringContent = new StringContent(json, Encoding.Default, "application/json");
            
            var response = await Client.PostAsync(route, stringContent);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
