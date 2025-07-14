using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FileHandling.Interface;
using FileHandling.Interfaces;
using FileHandling.Models;

namespace FileHandling.Services;

public class FileHandler : IFileHandler
{
    private BlobContainerClient _containerClient;
    private readonly IConfiguration _configuration;

    public FileHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private async Task UpdateContainerClient()
    {
        var blobUrl = _configuration["Azure:KeyVault"];
        SecretClient secretClient = new SecretClient(new Uri(blobUrl), new DefaultAzureCredential());
        KeyVaultSecret secret = await secretClient.GetSecretAsync("SasUrl");
        var blobUrlValue = secret.Value;
        _containerClient = new BlobContainerClient(new Uri(blobUrlValue));
    }

    public async Task<Stream> FileDownload(string name)
    {
        await UpdateContainerClient();
        var blobclient = _containerClient.GetBlobClient(name);
        if (!await blobclient.ExistsAsync())
            throw new FileNotFoundException($"File with name : {name} not found");
        var downloadInfo = await blobclient.DownloadStreamingAsync();
        return downloadInfo.Value.Content;
    }

    public async Task<string> FileUpload(Stream file, string filename)
    {
        await UpdateContainerClient();
        var blobClient = _containerClient.GetBlobClient(filename);
        await blobClient.UploadAsync(file);
        return "Uploaded";
    }

}
