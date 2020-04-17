using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
