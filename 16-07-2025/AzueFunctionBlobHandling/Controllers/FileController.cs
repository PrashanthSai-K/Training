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
            using var stream = file.OpenReadStream();
            var result = await _fileHandler.FileUpload(stream, filename);

            if (result == "Uploaded")
                return Ok("File uploaded sucessfully");

            return BadRequest(result);
        }

        [HttpGet("download")]
        public async Task<ActionResult> DownloadFile(string filename)
        {
            var stream = await _fileHandler.FileDownload(filename);
            if (stream == null)
                return BadRequest("File not found");

            return File(stream, "application/octet-stream", filename);
        }
    }
}
