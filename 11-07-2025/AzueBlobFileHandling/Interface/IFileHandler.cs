using System;
using System.Reflection.Metadata;

namespace FileHandling.Interface;

public interface IFileHandler
{
    Task<string> FileUpload(Stream file, string filename);
    Task<Stream> FileDownload(string name);

}
