using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive
{
    public class OcrScannerService : IOcrScannerService
    {
        private readonly ComputerVisionClient _computerVisionClient;
        private readonly ILogger<OcrScannerService> _log;

        public OcrScannerService(ComputerVisionClient computerVisionClient,
                                 ILogger<OcrScannerService> log)
        {
            _computerVisionClient = computerVisionClient
                    ?? throw new ArgumentNullException(nameof(computerVisionClient));
            _log = log
                    ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<string> ScanDocumentAndGetResults(string documentUrl)
        {
            BatchReadFileHeaders batchReadFileHeaders = await _computerVisionClient.BatchReadFileAsync(documentUrl);
            string operationId = GetScanOperationResultId(batchReadFileHeaders.OperationLocation);
            var readOperationResult = await ReadDocumentScanResult(operationId);

            StringBuilder sb = new StringBuilder();
            foreach (var recognitionResult in readOperationResult.RecognitionResults)
            {
                foreach (var line in recognitionResult.Lines)
                {
                    sb.AppendLine(line.Text);
                }
            }

            return sb.ToString();
        }

        private string GetScanOperationResultId(string operationLocation)
        {
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);
            return operationId;
        }

        private async Task<ReadOperationResult> ReadDocumentScanResult(string operationId)
        {
            int i = 0;
            int maxRetries = 20;
            ReadOperationResult results;
            do
            {
                results = await _computerVisionClient.GetReadOperationResultAsync(operationId);
                _log.LogInformation("Server status: {0}, waiting {1} seconds...", results.Status, i);
                await Task.Delay(2000);
                if (i == 9)
                {
                    _log.LogError("Server timed out. Cannot get OCR scanning result.");
                }
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

            return results;
        }
    }
}
