using System;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
