using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Data.Interfaces
{
    public interface IDataService<T> where T : class, IEntity
    {
        Task<T> AddAsync(T newEntity);
    }
}
