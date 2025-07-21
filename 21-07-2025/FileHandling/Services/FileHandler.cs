using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FileHandling.Interface;
using FileHandling.Interfaces;
using FileHandling.Models;

namespace FileHandling.Services;

public class FileHandler : IFileHandler
{
    private readonly IRepository<int, UploadedFile> _fileRepo;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;

    public FileHandler(IRepository<int, UploadedFile> fileRepository, IConfiguration configuration, BlobServiceClient blobServiceClient)
    {
        _fileRepo = fileRepository;
        _blobServiceClient = blobServiceClient;
        _containerClient = new BlobContainerClient(new Uri(configuration["Azure:StorageUrl"] ?? throw new Exception("Sas url not found")));
    }

    public async Task<UploadedFile> FileUpload(FileUploadDto fileUploadDto)
    {
        using var memoryStream = fileUploadDto.file.OpenReadStream();
        // await fileUploadDto.file.CopyToAsync(memoryStream);
        var blob = _containerClient.GetBlobClient(fileUploadDto.Title + ".mp4");
        await blob.UploadAsync(memoryStream);
        var newfile = new UploadedFile()
        {
            Title = fileUploadDto.Title,
            Description = fileUploadDto.Description,
            BlobUrl = $"/api/file/stream/{fileUploadDto.Title}.mp4",
            UploadedAt = DateTime.UtcNow
        };

        await _fileRepo.Create(newfile);
        return newfile;
    }

    public async Task<IEnumerable<UploadedFile>> GetList()
    {
        var files = await _fileRepo.GetAll();
        return files;
    }

    public StreamResponseDto GetSasUrl(string filename)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("videos");
        var blobClient = containerClient.GetBlobClient(filename);

        if (!blobClient.Exists())
            throw new Exception("File not found in blob storage");


        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = "videos",
            BlobName = filename,
            Resource = "b", // "b" = blob, "c" = container
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };

        // Grant read permissions
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        // Build the URI with SAS
        var sasUri = blobClient.GenerateSasUri(sasBuilder);

        return (new StreamResponseDto
        {
            FileName = filename,
            SasUrl = sasUri.ToString()
        });
    }
}
