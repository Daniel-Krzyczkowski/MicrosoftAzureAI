using Azure;
using Azure.Cosmos;
using AzureAI.CallCenterTalksAnalysis.Core.Model;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Data
{
    public sealed class CosmosDbDataService<T> : IDataService<T> where T : class, IEntity
    {
        private readonly ICosmosDbDataServiceConfiguration _dataServiceConfiguration;
        private readonly CosmosClient _client;
        private readonly ILogger<CosmosDbDataService<T>> _log;

        public CosmosDbDataService(ICosmosDbDataServiceConfiguration dataServiceConfiguration,
                                   CosmosClient client,
                                   ILogger<CosmosDbDataService<T>> log)
        {
            _dataServiceConfiguration = dataServiceConfiguration
                    ?? throw new ArgumentNullException(nameof(dataServiceConfiguration));
            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
            _log = log
                    ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
                return createResponse.Value;
            }
            catch (CosmosException ex)
            {
                _log.LogError($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                CosmosContainer container = GetContainer();

                await container.DeleteItemAsync<T>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));
            }
            catch (CosmosException ex)
            {
                _log.LogError($"Entity with ID: {entity.Id} was not removed successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }
            }
        }

        public async Task<T> GetAsync(string entityId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));
                return entityResult.Value;
            }
            catch (CosmosException ex)
            {
                _log.LogError($"Entity with ID: {entityId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<IEntity> entityResult = await container
                                                           .ReadItemAsync<IEntity>(entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));

                if (entityResult != null)
                {
                    await container
                          .ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));
                }
                return entity;
            }
            catch (CosmosException ex)
            {
                _log.LogError($"Entity with ID: {entity.Id} was not updated successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                CosmosContainer container = GetContainer();
                AsyncPageable<T> queryResultSetIterator = container.GetItemQueryIterator<T>();
                List<T> entities = new List<T>();

                await foreach (var entity in queryResultSetIterator)
                {
                    entities.Add(entity);
                }

                return entities;

            }
            catch (CosmosException ex)
            {
                _log.LogError($"Entities was not retrieved successfully - error details: {ex.Message}");

                if (ex.ErrorCode != "404")
                {
                    throw;
                }

                return null;
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
