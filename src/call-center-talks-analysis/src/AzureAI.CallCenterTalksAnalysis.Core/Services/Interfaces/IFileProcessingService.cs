using AzureAI.CallCenterTalksAnalysis.Core.Model;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces
{
    public interface IFileProcessingService
    {
        Task<FileAnalysisResult> AnalyzeFileContentAsync(InputFileData inputFileData);
    }
}
