using System.Linq;
using System.Threading.Tasks;
using HCrawler.Api.ViewModels;
using HCrawler.Core;
using Microsoft.AspNetCore.Mvc;

namespace HCrawler.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly Image.Image _image;

        public HomeController(Image.Image image)
        {
            _image = image;
        }

        public async Task<IActionResult> Index([FromQuery] PageFilter pageFilter)
        {
            var images = await _image.getAllAsync(pageFilter.ToRecord());
            var page = new Page(images, pageFilter.Checkpoint,pageFilter.Profile, pageFilter.Source);
            return View(page);
        }
    }
}
