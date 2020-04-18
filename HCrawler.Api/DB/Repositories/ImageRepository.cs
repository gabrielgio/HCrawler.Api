using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace HCrawler.Api.DB.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageDbContext _context;

        public ImageRepository(ImageDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Core.Repositories.Models.Image> GetAll()
        {
            return _context.Images
                .Include(x => x.Profile)
                .ThenInclude(x => x.Source)
                .Select(x => new Core.Repositories.Models.Image(x.Id, x.Path,
                    new Core.Repositories.Models.Profile(x.Profile.Name, x.Profile.Url,
                        new Core.Repositories.Models.Source(x.Profile.Source.Name, x.Profile.Source.Url))));
        }

        public async Task CreateImageAsync(CreateImage image)
        {
            var ext = image.Type == "image" ? "jpeg" : "mp4";
            var path = $"{image.ProfileName}/{image.Id}.{ext}";
            var query = _context.Images
                .Where(x => x.Path == path);

            var any = await query
                .AnyAsync();

            if (!any)
            {
                var profileId = await ProfileCreateIfNotExist(image);
                _context.Images.Add(new Image
                {
                    Path = path,
                    ProfileId = profileId,
                    Url = image.PostUrl,
                    CreatedOn = image.CreatedOn
                });
                await _context.SaveChangesAsync();
            }
        }


        private async Task<int> SourceCreateIfNotExist(CreateImage image)
        {
            var query = _context.Sources
                .Where(x => x.Name == image.SourceName);

            var any = await query
                .AnyAsync();

            if (!any)
            {
                var entityEntry = _context.Sources.Add(new Source
                {
                    Name = image.SourceName,
                    Url = image.SourceUrl
                });
                await _context.SaveChangesAsync();
                return entityEntry.Entity.Id;
            }

            var sourceDb = await query.FirstOrDefaultAsync();
            sourceDb.Url = image.SourceUrl;
            await _context.SaveChangesAsync();
            return sourceDb.Id;
        }

        private async Task<int> ProfileCreateIfNotExist(CreateImage image)
        {
            var query = _context.Profiles
                .Where(x => x.Name == image.ProfileName);

            var any = await query
                .AnyAsync();

            if (!any)
            {
                var sourceId = await SourceCreateIfNotExist(image);
                var entityEntry = _context.Profiles.Add(new Profile
                {
                    Name = image.ProfileName,
                    Url = image.ProfileUrl,
                    SourceId = sourceId
                });
                await _context.SaveChangesAsync();
                return entityEntry.Entity.Id;
            }

            var profileDb = await query.FirstOrDefaultAsync();
            profileDb.Url = image.ProfileUrl;
            await _context.SaveChangesAsync();
            return profileDb.Id;
        }
    }
}