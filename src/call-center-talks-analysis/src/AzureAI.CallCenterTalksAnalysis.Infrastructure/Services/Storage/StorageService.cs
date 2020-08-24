using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IStorageServiceConfiguration _storageServiceConfiguration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ILogger<StorageService> _log;
        public StorageService(IStorageServiceConfiguration storageServiceConfiguration,
                              BlobServiceClient blobServiceClient,
                              ILogger<StorageService> log)
        {
            _storageServiceConfiguration = storageServiceConfiguration
                    ?? throw new ArgumentNullException(nameof(storageServiceConfiguration));
            _blobServiceClient = blobServiceClient
                    ?? throw new ArgumentNullException(nameof(blobServiceClient));
            _log = log
                    ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task DeleteBlobIfExistsAsync(string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);
                await blockBlob.DeleteIfExistsAsync();
            }
            catch (RequestFailedException ex)
            {
                _log.LogError($"Document {blobName} was not deleted successfully - error details: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DoesBlobExistAsync(string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);
                var doesBlobExist = await blockBlob.ExistsAsync();
                return doesBlobExist.Value;
            }
            catch (RequestFailedException ex)
            {
                _log.LogError($"Document {blobName} existence cannot be verified - error details: {ex.Message}");
                throw;
            }
        }

        public async Task DownloadBlobIfExistsAsync(Stream stream, string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);

                var doesBlobExist = await blockBlob.ExistsAsync();

                if (doesBlobExist.Value == true)
                {
                    await blockBlob.DownloadToAsync(stream);
                }
            }

            catch (RequestFailedException ex)
            {
                _log.LogError($"Cannot download document {blobName} - error details: {ex.Message}");
                throw;
            }
        }

        public async Task<string> GetBlobUrl(string blobName)
        {
            try
            {
                var container = await GetBlobContainer();
                var blockBlob = container.GetBlobClient(blobName);

                var exists = await blockBlob.ExistsAsync();

                string blobUrl = exists ? blockBlob.Uri.ToString() : string.Empty;
                return blobUrl;
            }
            catch (RequestFailedException ex)
            {
                _log.LogError($"Url for document {blobName} was not found - error details: {ex.Message}");
                throw;
            }
        }

        public async Task UploadBlobAsync(Stream stream, string blobName)
        {
            try
            {
                stream.Seek(0, SeekOrigin.Begin);
                var container = await GetBlobContainer();

                BlobClient blob = container.GetBlobClient(blobName);
                await blob.UploadAsync(stream);
            }

            catch (RequestFailedException ex)
            {
                _log.LogError($"Document {blobName} was not uploaded successfully - error details: {ex.Message}");
                throw;
            }
        }

        private async Task<BlobContainerClient> GetBlobContainer()
        {
            try
            {
                BlobContainerClient container = _blobServiceClient
                                .GetBlobContainerClient(_storageServiceConfiguration.ContainerName);

                await container.CreateIfNotExistsAsync();

                return container;
            }
            catch (RequestFailedException ex)
            {
                _log.LogError($"Cannot find blob container: {_storageServiceConfiguration.ContainerName} - error details: {ex.Message}");
                throw;
            }
        }

        public string GenerateSasTokenForContainer()
        {
            BlobSasBuilder builder = new BlobSasBuilder();
            builder.BlobContainerName = _storageServiceConfiguration.ContainerName;
            builder.ContentType = "video/mp4";
            builder.SetPermissions(BlobAccountSasPermissions.Read);
            builder.StartsOn = DateTimeOffset.UtcNow;
            builder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(90);
            var sasToken = builder
                        .ToSasQueryParameters(new StorageSharedKeyCredential(_storageServiceConfiguration.AccountName,
                                                                             _storageServiceConfiguration.Key))
                        .ToString();
            return sasToken;
        }
    }
}
