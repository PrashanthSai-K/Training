using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerSupport.Controllers
{
    [Route("api/v1/chat/{chatId}/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromRoute] int chatId, [FromForm] ImageUploadDto imageDto)
        {
            var result = await _imageService.UploadImage(chatId, imageDto);
            return Ok(result);
        }

        [HttpGet("{imageName}")]
        public async Task<IActionResult> DownloadImage([FromRoute] int chatId, string imageName)
        {
            var result = await _imageService.DownloadImage(chatId, imageName);
            return Ok(result);
        }
    }
}
