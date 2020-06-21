using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Functions
{
    public class MediaFileTrigger
    {
        [FunctionName("func-media-file-trigger")]
        public async Task Run([BlobTrigger("files-for-analysis/{name}", Connection = "BlobStorageConnectionString")] Stream fileForAnalysis,
                                                                                         string name,
                                                                                         Uri uri,
                                                                                         [DurableClient] IDurableOrchestrationClient starter,
                                                                                         ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {fileForAnalysis.Length} Bytes");

            string instanceId = await starter.StartNewAsync("func-file-analysis-orchestrator", null, uri.ToString());

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
        }
    }
}
