using System.IO;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage.Interfaces
{
    public interface IStorageService
    {
        Task DownloadBlobIfExistsAsync(Stream stream, string blobName);
        Task UploadBlobAsync(Stream stream, string blobName);
        Task DeleteBlobIfExistsAsync(string blobName);
        Task<bool> DoesBlobExistAsync(string blobName);
        Task<string> GetBlobUrl(string blobName);
        string GenerateSasToken();
    }
}
