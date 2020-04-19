using System.Collections.Generic;
using System.Threading.Tasks;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;

namespace HCrawler.Core
{
    public class Image
    {
        private readonly IImageRepository _imageRepository;

        public Image(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public IEnumerable<DetailedImage> GetAll()
        {
            return _imageRepository.GetAll();
        }

        public async Task CreateImageIfNotExistsAsync(CreateImage createImage)
        {
            var profileId = await CreateProfileIfNotExists(createImage);

            var exists = await _imageRepository.ImageExistsAsync(createImage.ImagePath);

            if (!exists)
            {
                await _imageRepository.StoreImageAsync(StoreImage.FromCreateImage(profileId, createImage));
            }
        }

        public async Task<int> CreateProfileIfNotExists(CreateImage createImage)
        {
            var sourceId = await CreateSourceIfNotExists(createImage);

            var exists = await _imageRepository.ProfileExistsAsync(createImage.ProfileName);

            if (exists)
            {
                return await _imageRepository.GetProfileIdByNameAsync(createImage.ProfileName);
            }

            return await _imageRepository.StoreProfileAsync(StoreProfile.FromCreatImage(sourceId, createImage));
        }

        public async Task<int> CreateSourceIfNotExists(CreateImage createImage)
        {
            var exists = await _imageRepository.SourceExistsAsync(createImage.SourceName);

            if (exists)
            {
                return await _imageRepository.GetSourceIdByNameAsync(createImage.SourceName);
            }

            return await _imageRepository.StoreSourceAsync(StoreSource.FromCreateImage(createImage));
        }
    }
}
