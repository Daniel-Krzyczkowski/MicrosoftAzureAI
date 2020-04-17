using System.Collections.Generic;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public class WebApiSkillResponse
    {
        public List<WebApiResponseRecord> Values { get; set; } = new List<WebApiResponseRecord>();
    }
}
