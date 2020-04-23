using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model;
using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Interfaces
{
    public interface IFormRecognizerService
    {
        Task<string> AnalyzeFormAsync(byte[] formDocument, string formDocumentUrl);
        Task<FormAnalysisResponse> GetFormAnalysisResultAsync(string formAnalysisResultEndpoint);
    }
}
