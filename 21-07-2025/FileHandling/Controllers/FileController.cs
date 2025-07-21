using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs.Models;
using FileHandling.Interface;
using FileHandling.Models;
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
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> UploadFile(FileUploadDto fileUploadDto)
        {
            if (fileUploadDto == null || fileUploadDto.file.Length == 0)
                return BadRequest("File Missing");

            var filename = fileUploadDto.file.FileName;

            var result = await _fileHandler.FileUpload(fileUploadDto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetFiles()
        {
            var files = await _fileHandler.GetList();
            return Ok(files);
        }

        [HttpGet("stream/{filename}")]
        public async Task<ActionResult> StreamFile(string filename)
        {
            var result = _fileHandler.GetSasUrl(filename);
            return Ok(result);
        }
    }
}
