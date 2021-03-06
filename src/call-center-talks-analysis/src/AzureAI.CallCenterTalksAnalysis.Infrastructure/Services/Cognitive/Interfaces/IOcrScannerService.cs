﻿using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive.Interfaces
{
    public interface IOcrScannerService
    {
        Task<string> ScanDocumentAndGetResultsAsync(string documentUrl);
    }
}
