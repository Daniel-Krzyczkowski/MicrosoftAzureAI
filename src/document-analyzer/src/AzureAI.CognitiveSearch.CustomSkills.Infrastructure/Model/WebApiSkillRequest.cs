using System.Collections.Generic;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public class WebApiSkillRequest
    {
        public List<WebApiRequestRecord> Values { get; set; } = new List<WebApiRequestRecord>();
    }
}
