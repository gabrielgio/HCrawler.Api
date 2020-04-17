using System.Collections.Generic;
using System.Threading.Tasks;
using HCrawler.Api.Repositories.Models;

namespace HCrawler.Api.Repositories
{
    public interface IImageRepository
    {
        IEnumerable<Image> GetAll();

        Task CreateImageAsync(Image image);
    }
}