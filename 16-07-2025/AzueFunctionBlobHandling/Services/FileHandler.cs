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
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FileHandler> _logger;


    public FileHandler(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<FileHandler> logger)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    private async Task<BlobClient> GetBlobClientWithSas(string fileName)
    {
        string functionUrl = $"https://saidotnetfunc.azurewebsites.net/api/generate-sas/{fileName}?code=LQ_X34oHBzAtwr2OkX16T0uPK4y8HSKs7NL1GKJpOVM6AzFuEUQ12A%3D%3D";
        var client = _httpClientFactory.CreateClient();
        var sasResponse = await client.GetAsync(functionUrl);
        if (!sasResponse.IsSuccessStatusCode)
        {
            var error = await sasResponse.Content.ReadAsStringAsync();
            _logger.LogError($"Failed to get SAS URL: {error}");
            throw new InvalidOperationException("Could not obtain SAS URL.");
        }

        var sasData = await sasResponse.Content.ReadFromJsonAsync<SasResponse>();
        if (sasData == null || string.IsNullOrWhiteSpace(sasData.sasUrl))
        {
            throw new InvalidOperationException("SAS URL response invalid.");
        }

        _logger.LogInformation($"SAS URL obtained: {sasData.sasUrl}");

        return new BlobClient(new Uri(sasData.sasUrl));
    }

    public async Task<Stream> FileDownload(string name)
    {
        var blobclient = await GetBlobClientWithSas(name);
        if (!await blobclient.ExistsAsync())
            throw new FileNotFoundException($"File with name : {name} not found");
        var downloadInfo = await blobclient.DownloadStreamingAsync();
        return downloadInfo.Value.Content;
    }

    public async Task<string> FileUpload(Stream file, string filename)
    {
        var blobClient = await GetBlobClientWithSas(filename);
        await blobClient.UploadAsync(file);
        return "Uploaded";
    }

}
