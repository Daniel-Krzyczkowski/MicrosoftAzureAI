using System.Collections.Generic;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public class WebApiResponseRecord
    {
        public string RecordId { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
        public List<WebApiErrorWarningContract> Errors { get; set; } = new List<WebApiErrorWarningContract>();
        public List<WebApiErrorWarningContract> Warnings { get; set; } = new List<WebApiErrorWarningContract>();
    }
}
