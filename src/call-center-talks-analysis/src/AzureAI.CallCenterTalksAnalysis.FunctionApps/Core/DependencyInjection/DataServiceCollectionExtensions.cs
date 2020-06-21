using Azure.Cosmos;
using Azure.Cosmos.Serialization;
using AzureAI.CallCenterTalksAnalysis.Core.Model;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Core.DependencyInjection
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var cosmoDbConfiguration = serviceProvider.GetRequiredService<ICosmosDbDataServiceConfiguration>();
            CosmosClientOptions cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString, cosmosClientOptions);
            CosmosDatabase database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName)
                                                   .GetAwaiter()
                                                   .GetResult();
            CosmosContainer container = database.CreateContainerIfNotExistsAsync(
                cosmoDbConfiguration.ContainerName,
                cosmoDbConfiguration.PartitionKeyPath,
                400)
                .GetAwaiter()
                .GetResult();

            services.AddSingleton(cosmosClient);

            services.AddSingleton<IDataService<FileAnalysisResult>, CosmosDbDataService<FileAnalysisResult>>();

            return services;
        }
    }
}
