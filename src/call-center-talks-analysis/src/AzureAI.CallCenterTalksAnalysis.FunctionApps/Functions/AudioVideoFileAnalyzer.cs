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
    internal class AudioVideoFileAnalyzer
    {
        private readonly IAudioVideoFileProcessingService _audioVideoFileProcessingService;

        public AudioVideoFileAnalyzer(IAudioVideoFileProcessingService audioVideoFileProcessingService)
        {
            _audioVideoFileProcessingService = audioVideoFileProcessingService
                          ?? throw new ArgumentNullException(nameof(audioVideoFileProcessingService));
        }

        [FunctionName(FunctionNamesRepository.AudioVideoFileAnalyzerFunc)]
        public async Task<FileAnalysisResult> AnalyzeAudioVideoFile([ActivityTrigger] InputFileData inputFileData, ILogger log)
        {
            var fileAnalysisResult = await _audioVideoFileProcessingService.AnalyzeFileContent(inputFileData);
            return fileAnalysisResult;
        }
    }
}
