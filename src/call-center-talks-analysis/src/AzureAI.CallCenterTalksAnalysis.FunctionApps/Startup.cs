using AzureAI.CallCenterTalksAnalysis.FunctionApps.Core.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

[assembly: FunctionsStartup(typeof(AzureAI.CallCenterTalksAnalysis.FunctionApps.Startup))]
namespace AzureAI.CallCenterTalksAnalysis.FunctionApps
{
    internal class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureSettings();

            builder.Services.AddAppConfiguration(_configuration);
            builder.Services.AddFileFormatValidationServices();
            builder.Services.AddStorageServices();
            builder.Services.AddDataServices();
            builder.Services.AddFileProcessingServices();
        }

        private void ConfigureSettings()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();
            _configuration = config;
        }
    }
}
