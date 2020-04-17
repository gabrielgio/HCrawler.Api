using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCrawler.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HCrawler.Api.DB.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ImageDbContext _context;

        public ImageRepository(ImageDbContext context) => _context = context;

        public IEnumerable<Api.Repositories.Models.Image> GetAll() =>
            _context.Images
                .Include(x => x.Profile)
                .ThenInclude(x => x.Source)
                .Select(x => new Api.Repositories.Models.Image(x.Id, x.Path,
                    new Api.Repositories.Models.Profile(x.Profile.Name, x.Profile.Url,
                        new Api.Repositories.Models.Source(x.Profile.Source.Name, x.Profile.Source.Url))));

        public async Task CreateImageAsync(Api.Repositories.Models.Image image)
        {
            var query = _context.Images
                .Where(x => x.Path == image.Path);

            var any = await query
                .AnyAsync();

            if (!any)
            {
                var profileId = await ProfileCreateIfNotExist(image.Profile);
                var entityEntry = _context.Images.Add(new Image
                {
                    Path = image.Path,
                    ProfileId = profileId
                });
                await _context.SaveChangesAsync();
            }
        }


        private async Task<int> SourceCreateIfNotExist(Api.Repositories.Models.Source source)
        {
            var query = _context.Sources
                .Where(x => x.Name == source.Name);

            var any = await query
                .AnyAsync();

            if (!any)
            {
                var entityEntry = _context.Sources.Add(new Source
                {
                    Name = source.Name,
                    Url = source.Url
                });
                await _context.SaveChangesAsync();
                return entityEntry.Entity.Id;
            }

            var sourceDb = await query.FirstOrDefaultAsync();
            sourceDb.Url = source.Url;
            await _context.SaveChangesAsync();
            return sourceDb.Id;
        }

        private async Task<int> ProfileCreateIfNotExist(Api.Repositories.Models.Profile profile)
        {
            var query = _context.Profiles
                .Where(x => x.Name == profile.Name);

            var any = await query
                .AnyAsync();

            if (!any)
            {
                var sourceId = await SourceCreateIfNotExist(profile.Source);
                var entityEntry = _context.Profiles.Add(new Profile
                {
                    Name = profile.Name,
                    Url = profile.Url,
                    SourceId = sourceId
                });
                await _context.SaveChangesAsync();
                return entityEntry.Entity.Id;
            }

            var profileDb = await query.FirstOrDefaultAsync();
            profileDb.Url = profile.Url;
            await _context.SaveChangesAsync();
            return profileDb.Id;
        }
    }
}