using System;
using System.Threading.Tasks;
using FileHandling.Interface;
using FileHandling.Interfaces;
using FileHandling.Models;

namespace FileHandling.Services;

public class FileHandler : IFileHandler
{
    private readonly IRepository<int, UploadedFile> _fileRepo;
    public FileHandler(IRepository<int, UploadedFile> fileRepository)
    {
        _fileRepo = fileRepository;
    }

    public async Task<byte[]?> FileDownload(string name)
    {
        var files = await _fileRepo.GetAll();
        var file = files.FirstOrDefault(f => f.FileName == name);
        return file != null ?  file.Content : null;
    }

    public async Task<string> FileUpload(IFormFile file, string filename)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            Console.WriteLine(file.Name);
            await _fileRepo.Create(new UploadedFile()
            {
                FileName = filename,
                Content = memoryStream.ToArray()
            });
            return "File uploaded";

        }
        catch (System.Exception e)
        {
            return e.Message;
        }
    }

}
