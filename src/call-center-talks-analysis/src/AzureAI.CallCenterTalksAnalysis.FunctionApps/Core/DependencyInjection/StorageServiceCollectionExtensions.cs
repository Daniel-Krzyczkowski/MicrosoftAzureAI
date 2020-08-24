using Azure.Storage.Blobs;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Core.DependencyInjection
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var storageConfiguration = serviceProvider.GetRequiredService<IStorageServiceConfiguration>();

            services.TryAddSingleton(factory => new BlobServiceClient(storageConfiguration.ConnectionString));
            services.AddSingleton<IStorageService, StorageService>();
            return services;
        }
    }
}
