using System.Threading.Tasks;
using HCrawler.Api.ViewModels;
using HCrawler.Core;
using Microsoft.AspNetCore.Mvc;

namespace HCrawler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly Image.Image _image;

        public ImagesController(Image.Image image)
        {
            _image = image;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageFilter filter)
        {
            var page = await _image.getAllAsync(filter.ToRecord());
            return Ok(page);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateImage createImage)
        {
            await _image.createImageIfNotExistsAsync(createImage.ToRecord());
            return Ok();
        }
    }
}
