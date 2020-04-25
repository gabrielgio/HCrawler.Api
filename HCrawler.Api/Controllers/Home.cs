using System.Linq;
using System.Threading.Tasks;
using HCrawler.Api.DB.Utils;
using HCrawler.Core;
using HCrawler.Core.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace HCrawler.Api.Controllers
{
    public class Home : Controller
    {
        private readonly Image _image;

        public Home(Image image)
        {
            _image = image;
        }

        public IActionResult Index([FromQuery] PageFilter pageFilter)
        {
            var images = _image.GetAll(pageFilter).ToList();
            foreach (var image in images)
            {
                image.Path = $"instagran/{image.Path}";
            }

            var page = new Page<DetailedImage>(images, pageFilter.Number - 1, pageFilter.Number + 1);
            return View(page);
        }
    }
}
