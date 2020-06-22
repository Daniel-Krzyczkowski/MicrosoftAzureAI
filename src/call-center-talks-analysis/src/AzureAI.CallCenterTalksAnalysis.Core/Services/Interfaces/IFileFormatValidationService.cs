using AzureAI.CallCenterTalksAnalysis.Core.Enums;

namespace AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces
{
    public interface IFileFormatValidationService
    {
        FileContentType VeriyFileContentType(string filePath);
    }
}
