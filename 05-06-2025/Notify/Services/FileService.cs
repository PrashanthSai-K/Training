using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Notify.Interfaces;
using Notify.Misc;
using Notify.Models;
using Notify.Models.Dto;

namespace Notify.Services;

public class FileService : IFileService
{
    private readonly IRepository<int, Upload> _fileRepository;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public FileService(IRepository<int, Upload> repository, IHubContext<NotificationHub> hubContext)
    {
        _fileRepository = repository;
        _notificationHub = hubContext;
    }
    public async Task<byte[]?> FileDownload(string name)
    {
        var files = await _fileRepository.GetAll();
        var file = files.FirstOrDefault(f => f.Filename == name) ?? throw new Exception("File not found");
        return file.Content;
    }

    public async Task<List<FileNameDto>> GetFilesList()
    {
        var files = await _fileRepository.GetAll() ?? throw new Exception("No files found in Database");
        var filenames = files.Select(f => new FileNameDto() { FileName = f.Filename }).ToList();
        return filenames;
    }

    public async Task<string> FileUpload(IFormFile file, string filename)
    {
        try
        {
            var files = await _fileRepository.GetAll();
            if(files.Any(f=>f.Filename == filename))
                throw new Exception("File name already exists");

            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var upload = new Upload()
            {
                Filename = filename,
                Content = memoryStream.ToArray()
            };

            await _fileRepository.Create(upload);
            await _notificationHub.Clients.All.SendAsync("FileUploaded", $"{filename} has been uploaded");
            return "Uploaded";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}
