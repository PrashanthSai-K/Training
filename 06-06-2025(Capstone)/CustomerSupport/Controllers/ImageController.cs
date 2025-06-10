using CustomerSupport.Interfaces;
using CustomerSupport.Models.Dto;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Agent, Customer")]
        public async Task<IActionResult> UploadImage([FromRoute] int chatId, [FromForm] ImageUploadDto imageDto)
        {
            var userId = User?.Identity?.Name ?? "";

            var result = await _imageService.UploadImage(userId, chatId, imageDto);
            return Ok(result);
        }

        [HttpGet("{imageName}")]
        [Authorize(Roles = "Agent, Customer")]
        public async Task<IActionResult> DownloadImage([FromRoute] int chatId, string imageName)
        {
            var userId = User?.Identity?.Name ?? "";

            var result = await _imageService.DownloadImage(userId, chatId, imageName);
            return Ok(result);
        }

    }
}
