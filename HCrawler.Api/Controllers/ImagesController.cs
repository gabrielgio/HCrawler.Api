using System.Threading.Tasks;
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
        public async Task<IActionResult> Get([FromQuery] Payloads.PageFilter filter)
        {
            var page = await _image.GetAllAsync(filter);

            return Ok(page);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Payloads.CreateImage createImage)
        {
            await _image.CreateImageIfNotExistsAsync(createImage);
            return Ok();
        }
    }
}
