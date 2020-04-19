using System;
using System.Threading.Tasks;
using HCrawler.Core;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;
using HCrawler.Test.Mock;
using Xunit;
using static HCrawler.Test.Mock.ImageRepositoryMock;

namespace HCrawler.Test
{
    public class ImageTest
    {
        
        [Theory]
        [InlineData("demo", 1)]
        public async Task CreateSourceIfNotExists_Exists(string sourceName, int sourceId)
        {
            var imageRepo = MockImageRepository()
                .Ensure()
                .SourceExistsAsync(sourceName, true)
                .GetSourceIdByNameAsync(sourceName, sourceId)
                .Spawn();

            var image = new Image(imageRepo);

            await image.CreateSourceIfNotExists(new CreateImage {SourceName = sourceName});
        }

        [Theory]
        [InlineData("demo", 1, "http://localhost/")]
        public async Task CreateSourceIfNotExists_NotExists(string sourceName, int sourceId, string sourceUrl)
        {
            var storeSource = new StoreSource(sourceName, sourceUrl);
            
             var imageRepo = MockImageRepository()
                 .Ensure()
                 .SourceExistsAsync(sourceName, false)
                 .StoreSourceAsync(storeSource, sourceId)
                 .Spawn();
 
             var image = new Image(imageRepo);
 
             await image.CreateSourceIfNotExists(new CreateImage {SourceName = sourceName});           
        }
    }
}
