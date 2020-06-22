using AzureAI.CallCenterTalksAnalysis.Core.Enums;
using AzureAI.CallCenterTalksAnalysis.Core.Model;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.FunctionApps.Utils;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Functions
{
    public class FileAnalysisOrchestrator
    {
        private readonly IFileFormatValidationService _fileFormatValidationService;

        public FileAnalysisOrchestrator(IFileFormatValidationService fileFormatValidationService)
        {
            _fileFormatValidationService = fileFormatValidationService
                        ?? throw new ArgumentNullException(nameof(fileFormatValidationService));
        }

        [FunctionName(FunctionNamesRepository.FileAnalysisOrchestratorFunc)]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var fileLocation = context.GetInput<string>();
            if (!string.IsNullOrEmpty(fileLocation))
            {
                var fileFormat = _fileFormatValidationService.VeriyFileContentType(fileLocation);
                var inputFileData = new InputFileData
                {
                    FileContentType = fileFormat,
                    FilePath = fileLocation
                };

                if (fileFormat == FileContentType.PDF)
                {
                    var analysisResult = await context.CallActivityAsync<FileAnalysisResult>(FunctionNamesRepository.TextFileAnalyzerFunc,
                                                                                             inputFileData);
                    await context.CallActivityAsync(FunctionNamesRepository.AnalysisResultAggregatorFunc, analysisResult);
                }

                if (fileFormat == FileContentType.MP3 || fileFormat == FileContentType.MP4)
                {
                    var analysisResult = await context.CallActivityAsync<FileAnalysisResult>(FunctionNamesRepository.AudioVideoFileAnalyzerFunc,
                                                                                             inputFileData);
                    await context.CallActivityAsync(FunctionNamesRepository.AnalysisResultAggregatorFunc, analysisResult);
                }

                if (fileFormat == FileContentType.Unsupported)
                {
                    log.LogError($"Unsupported file content type. File path: {fileLocation}");
                }
            }
        }
    }
}