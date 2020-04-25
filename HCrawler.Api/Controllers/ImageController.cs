using System.Threading.Tasks;
using HCrawler.Core;
using HCrawler.Core.Repositories.Models;
using Microsoft.AspNetCore.Mvc;

namespace HCrawler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly Image _image;

        public ImageController(Image image)
        {
            _image = image;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]PageFilter filter)
        {
            var page = _image
                .GetAll()
                .Paginate(filter);
            
            return Ok(page);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateImage createImage)
        {
            await _image.CreateImageIfNotExistsAsync(createImage);
            return Ok();
        }
    }
}
