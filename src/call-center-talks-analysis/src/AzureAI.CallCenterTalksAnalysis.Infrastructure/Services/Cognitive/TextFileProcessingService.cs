using Azure;
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
            _textAnalyticsClient = textAnalyticsClient
                    ?? throw new ArgumentNullException(nameof(textAnalyticsClient));
            _storageService = storageService
                    ?? throw new ArgumentNullException(nameof(storageService));
            _ocrScannerService = ocrScannerService
                    ?? throw new ArgumentNullException(nameof(ocrScannerService));
            _log = log
                    ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<FileAnalysisResult> AnalyzeFileContentAsync(InputFileData inputFileData)
        {
            if (inputFileData.FileContentType == FileContentType.PDF)
            {
                var sasToken = _storageService.GenerateSasTokenForContainer();
                inputFileData.FilePath = $"{inputFileData.FilePath}?{sasToken}";

                var textFromTheInputDocument = await _ocrScannerService.ScanDocumentAndGetResultsAsync(inputFileData.FilePath);
                try
                {
                    DocumentSentiment sentimentAnalysisResult = await _textAnalyticsClient.AnalyzeSentimentAsync(textFromTheInputDocument);
                    var fileAnalysisResult = new FileAnalysisResult();
                    fileAnalysisResult.SentimentValues.Add(sentimentAnalysisResult.Sentiment.ToString());
                    return fileAnalysisResult;
                }
                catch (RequestFailedException ex)
                {
                    _log.LogError($"An error occurred when analyzing sentiment with {nameof(TextAnalyticsClient)} service", ex);
                }
            }

            throw new ArgumentException("Input file shuld be either TXT or PDF file.");
        }
    }
}
