using AzureAI.CallCenterTalksAnalysis.Core.Services;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AzureAI.CallCenterTalksAnalysis.FunctionApps.Core.DependencyInjection
{
    public static class FileFormatValidationServiceExtensions
    {
        public static IServiceCollection AddFileFormatValidationServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileFormatValidationService, FileFormatValidationService>();
            return services;
        }
    }
}
