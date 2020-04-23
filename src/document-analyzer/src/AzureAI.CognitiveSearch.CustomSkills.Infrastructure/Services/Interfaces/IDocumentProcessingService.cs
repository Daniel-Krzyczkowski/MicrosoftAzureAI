using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Interfaces
{
    public interface IDocumentProcessingService
    {
        Task<IList<WebApiRequestRecord>> DeserializeRequestAsync(HttpRequest request);
        Task<WebApiSkillResponse> ProcessInvoicesRecordsAsync(IEnumerable<WebApiRequestRecord> requestRecords);
    }
}
