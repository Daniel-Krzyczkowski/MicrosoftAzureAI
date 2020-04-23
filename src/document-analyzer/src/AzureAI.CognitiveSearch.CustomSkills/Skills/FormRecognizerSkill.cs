using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Constants;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills
{
    public class FormRecognizerSkill
    {
        private readonly IDocumentProcessingService _documentProcessingService;

        public FormRecognizerSkill(IDocumentProcessingService documentProcessingService)
        {
            _documentProcessingService = documentProcessingService;
        }

        [FunctionName(ServiceConstants.FormAnalyzerServiceName)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"{ServiceConstants.FormAnalyzerServiceName} Custom Skill: C# HTTP trigger function processed a request.");

            IList<WebApiRequestRecord> requestRecords = await _documentProcessingService.DeserializeRequestAsync(req);
            if (requestRecords == null)
            {
                return new BadRequestObjectResult($"{ServiceConstants.FormAnalyzerServiceName} - Invalid request record array.");
            }

            WebApiSkillResponse response = await _documentProcessingService.ProcessInvoicesRecordsAsync(requestRecords);
            return new OkObjectResult(response);
        }
    }
}
