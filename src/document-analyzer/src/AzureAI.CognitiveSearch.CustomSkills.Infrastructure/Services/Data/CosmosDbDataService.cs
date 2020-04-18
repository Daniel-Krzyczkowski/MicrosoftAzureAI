using Azure.Cosmos;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Data.Interfaces;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Data
{
    public sealed class CosmosDbDataService<T> : IDataService<T> where T : class, IEntity
    {
        private readonly CosmosDbSettings _dataServiceConfiguration;
        private readonly CosmosClient _client;
        private readonly ILogger<CosmosDbDataService<T>> _logger;

        public CosmosDbDataService(CosmosDbSettings dataServiceConfiguration, CosmosClient client,
                                                                                               ILogger<CosmosDbDataService<T>> logger)
        {
            _dataServiceConfiguration = dataServiceConfiguration;
            _client = client;
            _logger = logger;
        }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                var container = GetContainer();
                newEntity.Id = Guid.NewGuid();
                ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
                return createResponse.Value;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");
                throw;
            }
        }


        private CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_dataServiceConfiguration.DatabaseName);
            var container = database.GetContainer(_dataServiceConfiguration.ContainerName);
            return container;
        }
    }
}
