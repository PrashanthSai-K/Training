using System;
using Notify.Models.Dto;

namespace Notify.Interfaces;

public interface IFileService
{
    Task<string> FileUpload(IFormFile file, string filename);
    Task<byte[]?> FileDownload(string name);
    Task<List<FileNameDto>> GetFilesList();

}
