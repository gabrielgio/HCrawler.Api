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

        public async Task<IActionResult> Index([FromQuery] Payloads.PageFilter pageFilter)
        {
            var images = await _image.getAllAsync(pageFilter);
            var page = new Page<Proxies.DetailedImage>(images, pageFilter.Checkpoint, images.LastOrDefault()?.CreatedOn, pageFilter.Name);
            return View(page);
        }
    }
}
