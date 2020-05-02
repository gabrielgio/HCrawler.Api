using System.Linq;
using System.Threading.Tasks;
using HCrawler.Api.ViewModels;
using HCrawler.Core;
using HCrawler.Core.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace HCrawler.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly Image _image;

        public HomeController(Image image)
        {
            _image = image;
        }

        public async Task<IActionResult> Index([FromQuery] PageFilter pageFilter)
        {
            var images = await _image.GetAllAsync(pageFilter);
            var page = new Page<DetailedImage>(images, pageFilter.Checkpoint, images.LastOrDefault()?.CreatedOn, pageFilter.Name);
            return View(page);
        }
    }
}
