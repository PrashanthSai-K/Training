using System;
using System.Reflection.Metadata;
using Azure.Storage.Blobs;
using FileHandling.Models;

namespace FileHandling.Interface;

public interface IFileHandler
{
    Task<UploadedFile> FileUpload(FileUploadDto fileUploadDto);
    Task<IEnumerable<UploadedFile>> GetList();
    StreamResponseDto GetSasUrl(string filename);
}
