using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FileHandling.Interface;
using FileHandling.Interfaces;
using FileHandling.Models;

namespace FileHandling.Services;

public class FileHandler : IFileHandler
{
    private readonly BlobContainerClient _containerClient;
    public FileHandler(IConfiguration configuration)
    {
        var sasUrl = configuration["Azure:ContainerUrl"] ?? throw new Exception("Azure Sas Url not found");
        _containerClient = new BlobContainerClient(new Uri(sasUrl));
    }

    public async Task<Stream> FileDownload(string name)
    {
        var blobclient = _containerClient.GetBlobClient(name);
        if (!await blobclient.ExistsAsync())
            throw new FileNotFoundException($"File with name : {name} not found");
        var downloadInfo = await blobclient.DownloadStreamingAsync();
        return downloadInfo.Value.Content;
    }

    public async Task<string> FileUpload(Stream file, string filename)
    {
        var blobClient = _containerClient.GetBlobClient(filename);
        await blobClient.UploadAsync(file);
        return "Uploaded";
    }

}
