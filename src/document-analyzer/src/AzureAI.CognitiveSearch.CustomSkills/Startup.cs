using Azure.Cosmos;
using Azure.Cosmos.Serialization;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Data;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Data.Interfaces;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Document;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Interfaces;
using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Settings;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(AzureAI.CognitiveSearch.CustomSkills.Startup))]
namespace AzureAI.CognitiveSearch.CustomSkills
{
    class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureSettings(builder);

            var serviceProvider = builder.Services.BuildServiceProvider();
            var cosmosDbSettings = serviceProvider.GetRequiredService<CosmosDbSettings>();
            CosmosClientOptions cosmosClientOptions = new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            CosmosClient cosmosClient = new CosmosClient(cosmosDbSettings.ConnectionString, cosmosClientOptions);
            CosmosDatabase database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbSettings.DatabaseName)
                                                   .GetAwaiter()
                                                   .GetResult();
            CosmosContainer container = database.CreateContainerIfNotExistsAsync(
                cosmosDbSettings.ContainerName,
                cosmosDbSettings.PartitionKeyPath,
                400)
                .GetAwaiter()
                .GetResult();

            builder.Services.AddSingleton(cosmosClient);
            builder.Services.AddSingleton(typeof(IDataService<InvoiceData>), typeof(CosmosDbDataService<InvoiceData>));

            builder.Services.AddSingleton<IDocumentProcessingService, DocumentProcessingService>();

            builder.Services.AddSingleton<IDocumentProcessingService, DocumentProcessingService>();
            builder.Services.AddSingleton<IDocumentContentExtractor, DocumentContentExtractor>();
            builder.Services.AddHttpClient<IFormRecognizerService, FormRecognizerService>();
            builder.Services.AddSingleton<IDocumentProcessingService, DocumentProcessingService>();
        }

        private void ConfigureSettings(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();
            _configuration = config;

            var formRecognizerSettings = new FormRecognizerSettings()
            {
                ApiEndpoint = _configuration["FormRecognizerSettings:ApiEndpoint"],
                ApiKey = _configuration["FormRecognizerSettings:ApiKey"],
                ModelId = _configuration["FormRecognizerSettings:ModelId"]
            };
            builder.Services.AddSingleton(formRecognizerSettings);

            var cosmosDbSettings = new CosmosDbSettings()
            {
                ConnectionString = _configuration["CosmosDbSettings:ConnectionString"],
                ContainerName = _configuration["CosmosDbSettings:ContainerName"],
                DatabaseName = _configuration["CosmosDbSettings:DatabaseName"],
                PartitionKeyPath = _configuration["CosmosDbSettings:PartitionKeyPath"]
            };
            builder.Services.AddSingleton(cosmosDbSettings);
        }
    }
}
