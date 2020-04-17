using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public class WebApiSkillResponse
    {
        public List<WebApiResponseRecord> Values { get; set; } = new List<WebApiResponseRecord>();
    }
}
