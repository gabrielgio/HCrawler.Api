using System.Collections.Generic;
using System.Threading.Tasks;
using HCrawler.Core.Repositories.Models;

namespace HCrawler.Core.Repositories
{
    public interface IImageRepository
    {
        IEnumerable<Image> GetAll();

        Task CreateImageAsync(CreateImage image);
    }
}