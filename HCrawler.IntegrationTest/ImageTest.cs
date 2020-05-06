using System;
using System.Net;
using System.Threading.Tasks;
using HCrawler.Core;
using Xunit;

namespace HCrawler.IntegrationTest
{
    public class ImageTest : WebFixture<TestStartup>
    {
        public ImageTest()
        {
            Init().Wait();
        }

        private async Task Init()
        {
            var image = GetService<Image.Image>();
            var createImage = new Payloads.CreateImage
            {
                CreatedOn = DateTime.Now,
                ImagePath = "/root/Pictures",
                ImageUrl = "https://duckduckgo.com",
                ProfileName = "SomeDude",
                ProfileUrl = "http://duckduckgo.com",
                SourceName = "SomePace",
                SourceUrl = "https://someplece.com/"
            };
            await image.CreateImageIfNotExistsAsync(createImage);
        }

        [Fact]
        public async Task GetAsync()
        {
            const string route = "/images";
            var response = await Client.GetAsync(route);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task GetAsyncWithParam()
        {
            const string route = "/images?name=SomeDude";
            var response = await Client.GetAsync(route);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
