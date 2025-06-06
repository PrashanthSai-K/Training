using System.Threading.Tasks;
using FileHandling.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FileHandling
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileHandler _fileHandler;
        public FileController(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File Missing");

            var filename = file.FileName;

            var result = await _fileHandler.FileUpload(file, filename);

            if (result == "File uploaded")
                return Ok("File uploaded sucessfully");

            return BadRequest(result);
        }

        [HttpGet("download")]
        public async Task<ActionResult> DownloadFile(string filename)
        {
            var bytes = await _fileHandler.FileDownload(filename);
            if (bytes == null || bytes.Length == 0)
                return BadRequest("File not found");

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filename, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(bytes, contentType, filename);
        }
    }
}
