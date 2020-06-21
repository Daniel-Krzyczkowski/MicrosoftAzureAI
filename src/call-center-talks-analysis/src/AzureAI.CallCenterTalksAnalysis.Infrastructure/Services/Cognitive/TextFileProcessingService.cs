using Azure.AI.TextAnalytics;
using AzureAI.CallCenterTalksAnalysis.Core.Enums;
using AzureAI.CallCenterTalksAnalysis.Core.Model;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive
{
    public class TextFileProcessingService : ITextFileProcessingService
    {
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly IStorageService _storageService;
        private readonly IOcrScannerService _ocrScannerService;
        private readonly ILogger<TextFileProcessingService> _log;

        public TextFileProcessingService(TextAnalyticsClient textAnalyticsClient,
                                         IStorageService storageService,
                                         IOcrScannerService ocrScannerService,
                                         ILogger<TextFileProcessingService> log)
        {
            _textAnalyticsClient = textAnalyticsClient;
            _storageService = storageService;
            _ocrScannerService = ocrScannerService;
            _log = log;
        }

        public async Task<FileAnalysisResult> AnalyzeFileContent(InputFileData inputFileData)
        {
            if (inputFileData.FileContentType == FileContentType.PDF)
            {
                var sasToken = _storageService.GenerateSasToken();
                inputFileData.FilePath = $"{inputFileData.FilePath}?{sasToken}";

                var textFromTheInputDocument = await _ocrScannerService.ScanDocumentAndGetResults(inputFileData.FilePath);
                var sentimentAnalysisResponse = await _textAnalyticsClient.AnalyzeSentimentAsync(textFromTheInputDocument);
                if (sentimentAnalysisResponse != null)
                {
                    var sentimentAnalysisResult = sentimentAnalysisResponse.Value;
                    var fileAnalysisResult = new FileAnalysisResult();
                    fileAnalysisResult.SentimentValues.Add(sentimentAnalysisResult.Sentiment.ToString());
                    return fileAnalysisResult;
                }
            }

            throw new ArgumentException("Input file shuld be either TXT or PDF file.");
        }
    }
}
