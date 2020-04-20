using System.Collections.Generic;
using System.Diagnostics.Contracts;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;
using Moq;

namespace HCrawler.Test.Mock
{
    public static class ImageRepositoryMock
    {
        public static Mock<IImageRepository> MockImageRepository() => new Mock<IImageRepository>(MockBehavior.Strict);

        public static T GetAll<T>(this T source, IEnumerable<DetailedImage> result) where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.GetAll())
                .Returns(result);

            return source;
        }

        public static T ProfileExistsAsync<T>(this T source, string profileName, bool result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.ProfileExistsAsync(profileName))
                .ReturnsAsync(result);

            return source;
        }

        public static T SourceExistsAsync<T>(this T source, string sourceName, bool result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.SourceExistsAsync(sourceName))
                .ReturnsAsync(result);

            return source;
        }

        public static T ImageExistsAsync<T>(this T source, string imagePath, bool result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.ImageExistsAsync(imagePath))
                .ReturnsAsync(result);

            return source;
        }

        public static T StoreProfileAsync<T>(this T source, StoreProfile storeProfile, int result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.StoreProfileAsync(It.Is<StoreProfile>(y =>
                    y.Name == storeProfile.Name &&
                    y.Url == storeProfile.Url &&
                    y.SourceId == storeProfile.SourceId)))
                .ReturnsAsync(result);

            return source;
        }

        public static T StoreSourceAsync<T>(this T source, StoreSource storeSource, int result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.StoreSourceAsync(It.Is<StoreSource>(y =>
                    y.Name == storeSource.Name &&
                    y.Url == storeSource.Url)))
                .ReturnsAsync(result);

            return source;
        }


        public static T StoreImageAsync<T>(this T source, StoreImage storeImage, int result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.StoreImageAsync(It.Is<StoreImage>(y =>
                    y.Path == storeImage.Path &&
                    y.Url == storeImage.Url &&
                    y.CreatedOn == storeImage.CreatedOn &&
                    y.ProfileId == storeImage.ProfileId)))
                .ReturnsAsync(result);

            return source;
        }

        public static T GetProfileIdByNameAsync<T>(this T source, string profileName, int result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.GetProfileIdByNameAsync(profileName))
                .ReturnsAsync(result);

            return source;
        }

        public static T GetSourceIdByNameAsync<T>(this T source, string sourceName, int result)
            where T : Mock<IImageRepository>
        {
            source
                .Setup(x => x.GetSourceIdByNameAsync(sourceName))
                .ReturnsAsync(result);

            return source;
        }
    }
}
