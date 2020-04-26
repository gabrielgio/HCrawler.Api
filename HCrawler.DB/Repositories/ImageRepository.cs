using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HCrawler.Api.DB.Utils;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;

namespace HCrawler.DB.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDbConnection _connection;

        public ImageRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<DetailedImage> GetAll(PageFilter pageFilter)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ProfileExistsAsync(string profileName)
        {
            _connection.Open();
            return _connection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM images WHERE Name = @1", profileName);
        }

        public Task<bool> SourceExistsAsync(string sourceName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ImageExistsAsync(string imagePath)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> StoreProfileAsync(StoreProfile storeProfile)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> StoreSourceAsync(StoreSource storeSource)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> StoreImageAsync(StoreImage storeImage)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetProfileIdByNameAsync(string profileName)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetSourceIdByNameAsync(string sourceName)
        {
            throw new System.NotImplementedException();
        }
    }
}
