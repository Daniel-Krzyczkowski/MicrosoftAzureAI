using AzureAI.CallCenterTalksAnalysis.Core.Enums;

namespace AzureAI.CallCenterTalksAnalysis.Core.Model
{
    public class InputFileData
    {
        public FileContentType FileContentType { get; set; }
        public string FilePath { get; set; }
    }
}
