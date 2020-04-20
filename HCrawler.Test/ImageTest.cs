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
            var mock = MockImageRepository()
                .SourceExistsAsync(sourceName, true)
                .GetSourceIdByNameAsync(sourceName, sourceId);

            var imageRepo = mock.Spawn();

            var image = new Image(imageRepo);

            await image.CreateSourceIfNotExists(new CreateImage {SourceName = sourceName});

            mock.VerifyAll();
        }

        [Theory]
        [InlineData("demo", 1, "http://localhost/")]
        public async Task CreateSourceIfNotExists_NotExists(string sourceName, int sourceId, string sourceUrl)
        {
            var storeSource = new StoreSource(sourceName, sourceUrl);

            var mock = MockImageRepository()
                .SourceExistsAsync(sourceName, false)
                .StoreSourceAsync(storeSource, sourceId);

            var imageRepo = mock.Spawn();

            var image = new Image(imageRepo);

            var newSourceId = await image.CreateSourceIfNotExists(new CreateImage
            {
                SourceName = sourceName, SourceUrl = sourceUrl
            });
            Assert.Equal(sourceId, newSourceId);

            mock.VerifyAll();
        }

        [Theory]
        [InlineData("source name", 1, "http://localhost/", "profile name", 2)]
        public async Task CreateProfileIfNotExistsAsync_Exists(string sourceName, int sourceId, string sourceUrl,
            string profileName, int profileId)
        {
            var mock = MockImageRepository()
                .SourceExistsAsync(sourceName, true)
                .GetSourceIdByNameAsync(sourceName, sourceId)
                .ProfileExistsAsync(profileName, true)
                .GetProfileIdByNameAsync(profileName, profileId);

            var imageRepo = mock.Spawn();

            var image = new Image(imageRepo);

            var newProfileId = await image.CreateProfileIfNotExistsAsync(new CreateImage
            {
                SourceName = sourceName, ProfileName = profileName,
            });

            Assert.Equal(profileId, newProfileId);

            mock.VerifyAll();
        }

        [Theory]
        [InlineData("source name", 1, "http://localhost/", "profile name", 3)]
        public async Task CreateProfileIfNotExistsAsync_NotExists(string sourceName, int sourceId, string url,
            string profileName, int profileId)
        {
            var storeProfile = new StoreProfile(sourceId, profileName, url);

            var mock = MockImageRepository()
                .SourceExistsAsync(sourceName, true)
                .GetSourceIdByNameAsync(sourceName, sourceId)
                .ProfileExistsAsync(profileName, false)
                .StoreProfileAsync(storeProfile, profileId);

            var imageRepo = mock.Spawn();

            var image = new Image(imageRepo);

            var resultProfileId = await image.CreateProfileIfNotExistsAsync(new CreateImage
            {
                SourceName = sourceName, ProfileName = profileName, ProfileUrl = url
            });

            Assert.Equal(profileId, resultProfileId);

            mock.VerifyAll();
        }

        [Theory]
        [InlineData("/home/", "demo", 2, "profile Name", 3)]
        public async Task CreateImageIfNotExistsAsync_Exists(string path, string sourceName, int sourceId,
            string profileName, int profileId)
        {
            var mock = MockImageRepository()
                .SourceExistsAsync(sourceName, true)
                .GetSourceIdByNameAsync(sourceName, sourceId)
                .ProfileExistsAsync(profileName, true)
                .GetProfileIdByNameAsync(profileName, profileId)
                .ImageExistsAsync(path, true);

            var imageRepo = mock.Spawn();

            var image = new Image(imageRepo);

            await image.CreateImageIfNotExistsAsync(new CreateImage
            {
                ImagePath = path, CreatedOn = DateTime.Now, SourceName = sourceName, ProfileName = profileName
            });

            mock.VerifyAll();
        }
    }
}
