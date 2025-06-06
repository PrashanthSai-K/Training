using System;
using System.Reflection.Metadata;

namespace FileHandling.Interface;

public interface IFileHandler
{
    Task<string> FileUpload(IFormFile file, string filename);
    Task<byte[]?> FileDownload(string name);

}
