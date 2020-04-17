using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services;
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
        }
    }
}
