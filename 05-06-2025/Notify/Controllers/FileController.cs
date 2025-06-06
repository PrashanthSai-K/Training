using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Notify.Interfaces;

namespace Notify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null)
                return BadRequest("File in missing");

            var result = await _fileService.FileUpload(file, file.FileName);
            if (result == "Uploaded")
                return Ok("File Uploaded Successfully");

            return BadRequest(result);
        }

        [HttpGet("files")]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _fileService.GetFilesList();
            return Ok(files);
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string filename)
        {
            var bytes = await _fileService.FileDownload(filename);
            if (bytes == null || bytes.Length == 0)
                return BadRequest("File curroupted");
            
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filename, out string? contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(bytes, contentType , filename);
        }
    }
}
