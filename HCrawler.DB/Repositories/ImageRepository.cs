using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HCrawler.Api.DB.Utils;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;
using HCrawler.DB.Repositories.DbModel;

namespace HCrawler.DB.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDbConnection _connection;

        public ImageRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        private string PushFilter(string name)
        {
            if (name is object)
            {
                return $@"WHERE P.""Name"" = '{name}'";
            }

            return string.Empty;
        }

        public async Task<IEnumerable<DetailedImage>> GetAll(PageFilter pageFilter)
        {
            var number = pageFilter.Size * pageFilter.Number;
            var sql = $@"
            SELECT I.""Id"" ImageId,
                   I.""Path"" ImagePath,
                   P.""Name"" ProfileName,
                   P.""Url"" ProfileUrl,
                   S.""Name"" SourceName,
                   S.""Url"" SourceUrl 
            FROM ""Images"" I
            INNER JOIN ""Profiles"" P on I.""ProfileId"" = P.""Id""
            INNER JOIN ""Sources"" S on P.""SourceId"" = S.""Id""
            {PushFilter(pageFilter.Name)}
            ORDER BY I.""CreatedOn"" DESC
            LIMIT @size
            OFFSET @number
            ";

            var dbImages = await _connection.QueryAsync<DbDetailedImage>(sql, new {number, size = pageFilter.Size});

            return dbImages.Select(x => x.ToDetailedImage());
        }

        public Task<bool> ProfileExistsAsync(string profileName)
        {
            var sql = @"
            SELECT COUNT(1) FROM ""Profiles""
            WHERE ""Name"" = @profileName
            ";
            return _connection.ExecuteScalarAsync<bool>(sql, new {profileName});
        }

        public Task<bool> SourceExistsAsync(string sourceName)
        {
            var sql = @"
            SELECT COUNT(1) FROM ""Sources""
            WHERE ""Name"" = @sourceName
            ";
            return _connection.ExecuteScalarAsync<bool>(sql, new {sourceName});
        }

        public Task<bool> ImageExistsAsync(string imagePath)
        {
            var sql = @"
            SELECT COUNT(1) FROM ""Images"" 
            WHERE ""Path"" = @imagePath
            ";
            return _connection.ExecuteScalarAsync<bool>(sql, new {imagePath});
        }

        public Task<int> StoreProfileAsync(StoreProfile storeProfile)
        {
            var sql = @"
            INSERT INTO ""Profiles"" (""Name"", ""SourceId"", ""Url"")
            VALUES (@name, @sourceId, @url)
            RETURNING ""Id""
            ";
            return _connection.QueryFirstAsync<int>(sql,
                new {name = storeProfile.Name, sourceId = storeProfile.SourceId, url = storeProfile.Url});
        }

        public Task<int> StoreSourceAsync(StoreSource storeSource)
        {
            var sql = @"
            INSERT INTO ""Sources"" (""Name"", ""Url"")
            VALUES (@name, @url)
            RETURNING ""Id""
            ";
            return _connection.QueryFirstAsync<int>(sql, new {name = storeSource.Name, url = storeSource.Url});
        }

        public Task<int> StoreImageAsync(StoreImage storeImage)
        {
            var sql = @"
            INSERT INTO  ""Images"" (""ProfileId"", ""Path"", ""Url"", ""CreatedOn"")
            VALUES (@profileId, @path, @url, @createdOn)
            RETURNING ""Id""
            ";
            return _connection.QueryFirstAsync<int>(sql,
                new
                {
                    profileId = storeImage.ProfileId,
                    path = storeImage.Path,
                    url = storeImage.Url,
                    createdOn = storeImage.CreatedOn
                });
        }

        public Task<int> GetProfileIdByNameAsync(string profileName)
        {
            var sql = @"
            SELECT P.""Id"" FROM ""Profiles"" P
            WHERE P.""Name"" = @profileName
            LIMIT 1
            ";
            return _connection.QuerySingleAsync<int>(sql, new {profileName});
        }

        public Task<int> GetSourceIdByNameAsync(string sourceName)
        {
            var sql = @"
            SELECT S.""Id"" FROM ""Sources"" S
            WHERE S.""Name"" = @sourceName
            LIMIT 1
            ";
            return _connection.QuerySingleAsync<int>(sql, new {sourceName});
        }
    }
}
