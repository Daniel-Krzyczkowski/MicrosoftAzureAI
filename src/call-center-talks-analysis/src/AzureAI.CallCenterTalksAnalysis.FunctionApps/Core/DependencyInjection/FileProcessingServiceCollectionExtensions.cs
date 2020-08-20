using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.TextAnalytics;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive.Interfaces;
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

            var formRecognizerServiceConfiguration = serviceProvider.GetRequiredService<IFormRecognizerServiceConfiguration>();
            services.TryAddSingleton(implementationFactory =>
            {
                var credential = new AzureKeyCredential(formRecognizerServiceConfiguration.ApiKey);
                var formRecognizerClient = new FormRecognizerClient(new Uri(formRecognizerServiceConfiguration.Endpoint), credential);

                return formRecognizerClient;
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
