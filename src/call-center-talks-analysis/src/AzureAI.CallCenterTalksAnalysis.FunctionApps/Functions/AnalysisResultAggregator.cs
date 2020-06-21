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
    internal class AnalysisResultAggregator
    {
        private readonly IDataService<FileAnalysisResult> _dataService;

        public AnalysisResultAggregator(IDataService<FileAnalysisResult> dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
        }

        [FunctionName(FunctionNamesRepository.AnalysisResultAggregatorFunc)]
        public async Task AggregateAnalysis([ActivityTrigger] FileAnalysisResult analysisResult, ILogger log)
        {
            if (analysisResult != null)
            {
                await _dataService.AddAsync(analysisResult);
            }

            else
            {
                log.LogError($"Analysis result is empty, cannot insert record.");
            }
        }
    }
}
