using AzureAI.CallCenterTalksAnalysis.Core.Enums;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using System;
using System.IO;

namespace AzureAI.CallCenterTalksAnalysis.Core.Services
{
    public class FileFormatValidationService : IFileFormatValidationService
    {
        public FileContentType VeriyFileContentType(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath);

            if (fileExtension.Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
            {
                return FileContentType.PDF;
            }

            else if (fileExtension.Equals(".mp4", StringComparison.InvariantCultureIgnoreCase))
            {
                return FileContentType.MP4;
            }

            else if (fileExtension.Equals(".mp3", StringComparison.InvariantCultureIgnoreCase))
            {
                return FileContentType.MP3;
            }

            else
            {
                return FileContentType.Unsupported;
            }
        }
    }
}
