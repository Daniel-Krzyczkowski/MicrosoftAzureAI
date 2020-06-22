using AzureAI.CallCenterTalksAnalysis.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces
{
    public interface IDataService<T> where T : class, IEntity
    {
        Task<T> AddAsync(T newEntity);
        Task<T> GetAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IReadOnlyList<T>> GetAllAsync();
    }
}
