using System.Linq;
using System.Threading.Tasks;
using HCrawler.Api.DB.Utils;
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
            var images = await _image.GetAll(pageFilter);
            foreach (var image in images)
            {
                image.Path = $"instagram/{image.Path}";
            }

            var page = new Page<DetailedImage>(images, pageFilter.Number - 1, pageFilter.Number + 1);
            return View(page);
        }
    }
}
