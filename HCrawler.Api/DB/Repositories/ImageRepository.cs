using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCrawler.Api.DB.Utils;
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

        public IEnumerable<DetailedImage> GetAll(PageFilter pageFilter)
        {
            return _context.Images
                .Include(x => x.Profile)
                .ThenInclude(x => x.Source)
                .OrderByDescending(x => x.CreatedOn)
                .Paginate(pageFilter)
                .Select(x => new DetailedImage(x.Id, x.Path,
                    new DetailedProfile(x.Profile.Name, x.Profile.Url,
                        new DetailedSource(x.Profile.Source.Name, x.Profile.Source.Url))));
        }

        public Task<bool> ProfileExistsAsync(string profileName)
        {
            return _context.Profiles.AnyAsync(x => x.Name == profileName);
        }

        public Task<bool> SourceExistsAsync(string sourceName)
        {
            return _context.Sources.AnyAsync(x => x.Name == sourceName);
        }

        public Task<bool> ImageExistsAsync(string imagePath)
        {
            return _context.Images.AnyAsync(x => x.Path == imagePath);
        }

        public async Task<int> StoreProfileAsync(StoreProfile storeProfile)
        {
            var entry = _context.Profiles.Add(new Profile
            {
                Name = storeProfile.Name, Url = storeProfile.Url, SourceId = storeProfile.SourceId
            });

            await _context.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task<int> StoreSourceAsync(StoreSource storeSource)
        {
            var entry = _context.Sources.Add(new Source {Name = storeSource.Name, Url = storeSource.Url});

            await _context.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public async Task<int> StoreImageAsync(StoreImage storeImage)
        {
            var entry = _context.Images.Add(new Image
            {
                Path = storeImage.Path,
                ProfileId = storeImage.ProfileId,
                Url = storeImage.Url,
                CreatedOn = storeImage.CreatedOn
            });

            await _context.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public Task<int> GetProfileIdByNameAsync(string profileName)
        {
            return _context.Profiles
                .Where(x => x.Name == profileName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> GetSourceIdByNameAsync(string sourceName)
        {
            return _context.Sources
                .Where(x => x.Name == sourceName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }

        public Task<int> GetImageIdByPath(string path)
        {
            return _context.Images
                .Where(x => x.Path == path)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }
    }
}
