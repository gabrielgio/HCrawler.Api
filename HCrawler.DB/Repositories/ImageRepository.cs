using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using HCrawler.Core;
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

        public async Task<IEnumerable<Proxies.Image>> getAllAsync(Payloads.PageFilter pageFilter)
        {
            var checkpoint = pageFilter.Checkpoint ?? DateTime.MaxValue;

            var sql = $@"
            SELECT I.""Id"" ImageId,
                   I.""Path"" ImagePath,
                   P.""Id"" ProfileId,
                   P.""Name"" ProfileName,
                   P.""Url"" ProfileUrl,
                   S.""Id"" SourceId,
                   S.""Name"" SourceName,
                   S.""Url"" SourceUrl,
                   I.""CreatedOn"" ImageCreatedOn,
                   I.""Url"" ImageUrl
            FROM ""Images"" I
            INNER JOIN ""Profiles"" P on I.""ProfileId"" = P.""Id""
            INNER JOIN ""Sources"" S on P.""SourceId"" = S.""Id""
            WHERE I.""CreatedOn"" < @checkpoint
            {PushNameFilter(pageFilter.Profile)}
            {PushSourceFilter(pageFilter.Source)}
            ORDER BY I.""CreatedOn"" DESC
            LIMIT @size
            ";

            var dbImages = await _connection.QueryAsync<DbDetailedImage>(sql, new {checkpoint, size = pageFilter.Size});

            return dbImages.Select(x => x.ToRecord());
        }

        public Task<bool> profileExistsAsync(string profileName)
        {
            var sql = @"
            SELECT COUNT(1) FROM ""Profiles""
            WHERE ""Name"" = @profileName
            ";
            return _connection.ExecuteScalarAsync<bool>(sql, new {profileName});
        }

        public Task<bool> sourceExistsAsync(string sourceName)
        {
            var sql = @"
            SELECT COUNT(1) FROM ""Sources""
            WHERE ""Name"" = @sourceName
            ";
            return _connection.ExecuteScalarAsync<bool>(sql, new {sourceName});
        }

        public Task<bool> imageExistsAsync(string imagePath)
        {
            var sql = @"
            SELECT COUNT(1) FROM ""Images"" 
            WHERE ""Path"" = @imagePath
            ";
            return _connection.ExecuteScalarAsync<bool>(sql, new {imagePath});
        }

        public Task<int> storeProfileAsync(Payloads.StoreProfile storeProfile)
        {
            var sql = @"
            INSERT INTO ""Profiles"" (""Name"", ""SourceId"", ""Url"")
            VALUES (@name, @sourceId, @url)
            RETURNING ""Id""
            ";
            return _connection.QueryFirstAsync<int>(sql,
                new {name = storeProfile.Name, sourceId = storeProfile.SourceId, url = storeProfile.Url});
        }

        public Task<int> storeSourceAsync(Payloads.StoreSource storeSource)
        {
            var sql = @"
            INSERT INTO ""Sources"" (""Name"", ""Url"")
            VALUES (@name, @url)
            RETURNING ""Id""
            ";
            return _connection.QueryFirstAsync<int>(sql, new {name = storeSource.Name, url = storeSource.Url});
        }

        public Task<int> storeImageAsync(Payloads.StoreImage storeImage)
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

        public Task<int> getProfileIdByNameAsync(string profileName)
        {
            var sql = @"
            SELECT P.""Id"" FROM ""Profiles"" P
            WHERE P.""Name"" = @profileName
            LIMIT 1
            ";
            return _connection.QuerySingleAsync<int>(sql, new {profileName});
        }

        public Task<int> getSourceIdByNameAsync(string sourceName)
        {
            var sql = @"
            SELECT S.""Id"" FROM ""Sources"" S
            WHERE S.""Name"" = @sourceName
            LIMIT 1
            ";
            return _connection.QuerySingleAsync<int>(sql, new {sourceName});
        }

        private string PushNameFilter(int? name)
        {
            if (name is object)
            {
                return $@"AND P.""Id"" = '{name.ToString()}'";
            }

            return string.Empty;
        }

        private string PushSourceFilter(int? source)
        {
            if (source is object)
            {
                return $@"AND S.""Id"" = '{source.ToString()}'";
            }

            return string.Empty;
        }
    }
}
