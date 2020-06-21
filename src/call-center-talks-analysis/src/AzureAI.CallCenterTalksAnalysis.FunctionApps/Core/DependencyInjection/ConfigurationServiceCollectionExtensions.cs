using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Core.DependencyInjection
{
    internal static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            //services.Configure<ApplicationInsightsServiceConfiguration>(config.GetSection("ApplicationInsightsSettings"));
            //services.AddSingleton<IValidateOptions<ApplicationInsightsServiceConfiguration>, ApplicationInsightsServiceConfigurationValidation>();
            //var applicationInsightsServiceConfigurationValidator = services.BuildServiceProvider().GetRequiredService<IValidateOptions<ApplicationInsightsServiceConfiguration>>();
            //var applicationInsightsServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationInsightsServiceConfiguration>>().Value;
            //applicationInsightsServiceConfigurationValidator.Validate(nameof(ApplicationInsightsServiceConfiguration), applicationInsightsServiceConfiguration);

            services.Configure<ApplicationInsightsServiceConfiguration>(config.GetSection("ApplicationInsightsSettings"));
            services.AddSingleton<IValidateOptions<ApplicationInsightsServiceConfiguration>, ApplicationInsightsServiceConfigurationValidation>();
            var applicationInsightsServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationInsightsServiceConfiguration>>().Value;
            services.AddSingleton<IApplicationInsightsServiceConfiguration>(applicationInsightsServiceConfiguration);

            services.Configure<StorageServiceConfiguration>(config.GetSection("BlobStorageSettings"));
            services.AddSingleton<IValidateOptions<StorageServiceConfiguration>, StorageServiceConfigurationValidation>();
            var storageServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<StorageServiceConfiguration>>().Value;
            services.AddSingleton<IStorageServiceConfiguration>(storageServiceConfiguration);

            services.Configure<CosmosDbDataServiceConfiguration>(config.GetSection("CosmosDbSettings"));
            services.AddSingleton<IValidateOptions<CosmosDbDataServiceConfiguration>, CosmosDbDataServiceConfigurationValidation>();
            var cosmosDbDataServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbDataServiceConfiguration>>().Value;
            services.AddSingleton<ICosmosDbDataServiceConfiguration>(cosmosDbDataServiceConfiguration);

            services.Configure<TextAnalyticsServiceConfiguration>(config.GetSection("TextAnalyticsServiceSettings"));
            services.AddSingleton<IValidateOptions<TextAnalyticsServiceConfiguration>, TextAnalyticsServiceConfigurationValidation>();
            var textAnalyticsServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<TextAnalyticsServiceConfiguration>>().Value;
            services.AddSingleton<ITextAnalyticsServiceConfiguration>(textAnalyticsServiceConfiguration);

            services.Configure<ComputerVisionServiceConfiguration>(config.GetSection("ComputerVisionServiceSettings"));
            services.AddSingleton<IValidateOptions<ComputerVisionServiceConfiguration>, ComputerVisionServiceConfigurationValidation>();
            var computerVisionServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ComputerVisionServiceConfiguration>>().Value;
            services.AddSingleton<IComputerVisionServiceConfiguration>(computerVisionServiceConfiguration);

            services.Configure<VideoIndexerServiceConfiguration>(config.GetSection("VideoIndexerServiceSettings"));
            services.AddSingleton<IValidateOptions<VideoIndexerServiceConfiguration>, VideoIndexerServiceConfigurationValidation>();
            var videoIndexerServiceConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<VideoIndexerServiceConfiguration>>().Value;
            services.AddSingleton<IVideoIndexerServiceConfiguration>(videoIndexerServiceConfiguration);


            return services;
        }
    }
}
