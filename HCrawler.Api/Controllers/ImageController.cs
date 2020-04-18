using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HCrawler.Core.Repositories;
using HCrawler.Core.Repositories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HCrawler.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        public IActionResult Get() =>
            Ok(_imageRepository.GetAll());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateImage createImage)
        {
            await _imageRepository.CreateImageAsync(createImage);
            return Ok();
        }
    }
}