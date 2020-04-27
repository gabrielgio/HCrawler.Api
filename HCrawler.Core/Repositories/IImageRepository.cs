using System.Collections.Generic;
using System.Threading.Tasks;
using HCrawler.Api.DB.Utils;
using HCrawler.Core.Repositories.Models;

namespace HCrawler.Core.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<DetailedImage>> GetAll(PageFilter pageFilter);
        Task<bool> ProfileExistsAsync(string profileName);
        Task<bool> SourceExistsAsync(string sourceName);
        Task<bool> ImageExistsAsync(string imagePath);
        Task<int> StoreProfileAsync(StoreProfile storeProfile);
        Task<int> StoreSourceAsync(StoreSource storeSource);
        Task<int> StoreImageAsync(StoreImage storeImage);
        Task<int> GetProfileIdByNameAsync(string profileName);
        Task<int> GetSourceIdByNameAsync(string sourceName);
    }
}
