using Azure;
using Azure.AI.TextAnalytics;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Core.DependencyInjection
{
    public static class FileProcessingServiceCollectionExtensions
    {
        public static IServiceCollection AddFileProcessingServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var textAnalyticsServiceConfiguration = serviceProvider.GetRequiredService<ITextAnalyticsServiceConfiguration>();
            services.TryAddSingleton(implementationFactory =>
            {
                TextAnalyticsClient textAnalyticsClient = new TextAnalyticsClient(new Uri(textAnalyticsServiceConfiguration.Endpoint),
                                                                                  new AzureKeyCredential(textAnalyticsServiceConfiguration.ApiKey));
                return textAnalyticsClient;
            });

            var computerVisionServiceConfiguration = serviceProvider.GetRequiredService<IComputerVisionServiceConfiguration>();
            services.TryAddSingleton(implementationFactory =>
            {
                var apiKeyServiceClientCredentials = new ApiKeyServiceClientCredentials(computerVisionServiceConfiguration.ApiKey);
                ComputerVisionClient computerVisionClient = new ComputerVisionClient(apiKeyServiceClientCredentials)
                {
                    Endpoint = computerVisionServiceConfiguration.Endpoint
                };

                return computerVisionClient;
            });
            services.AddSingleton<IOcrScannerService, OcrScannerService>();

            services.AddHttpClient<IAudioVideoFileProcessingService, AudioVideoFileProcessingService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
                {
                    AllowAutoRedirect = false
                });
            services.AddSingleton<ITextFileProcessingService, TextFileProcessingService>();

            return services;
        }
    }
}
