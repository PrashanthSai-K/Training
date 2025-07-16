using System;
using System.Net;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class SasUrlRotater
    {
        private readonly ILogger _logger;

        public SasUrlRotater(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SasUrlRotater>();
        }

        [Function("function")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "generate-sas/{blobName}")] HttpRequestData req,
            string blobName)
        {
            _logger.LogInformation($"Generating SAS for blob: {blobName}");

            string connectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            string keyVaultUri = Environment.GetEnvironmentVariable("KeyVaultUri");

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(keyVaultUri))
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Missing configuration.");
                return errorResponse;
            }

            var blobServiceClient = new BlobServiceClient(connectionString);
            var accountName = blobServiceClient.AccountName;

            string accountKey = null;
            foreach (var part in connectionString.Split(';'))
            {
                if (part.StartsWith("AccountKey=", StringComparison.OrdinalIgnoreCase))
                {
                    accountKey = part.Substring("AccountKey=".Length);
                    break;
                }
            }

            if (string.IsNullOrEmpty(accountKey))
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("AccountKey not found.");
                return errorResponse;
            }

            var credential = new StorageSharedKeyCredential(accountName, accountKey);
            var blobClient = blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);

            var expiresOn = DateTimeOffset.UtcNow.AddHours(1);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = expiresOn
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.Write);

            var sasUri = blobClient.GenerateSasUri(sasBuilder);

            var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
            var secret = new KeyVaultSecret("myblobsecret", sasUri.ToString());
            secret.Properties.Tags.Add("ExpiresOn", expiresOn.UtcDateTime.ToString("o"));

            await secretClient.SetSecretAsync(secret);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { sasUrl = sasUri.ToString(), expiresOn });

            return response;
        }
    }
}
