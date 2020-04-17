using System.Collections.Generic;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public class WebApiRequestRecord
    {
        public string RecordId { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}
