using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Interfaces
{
    public interface IDocumentContentExtractor
    {
        Task<byte[]> DownloadDocument(string documentUrl);
    }
}
