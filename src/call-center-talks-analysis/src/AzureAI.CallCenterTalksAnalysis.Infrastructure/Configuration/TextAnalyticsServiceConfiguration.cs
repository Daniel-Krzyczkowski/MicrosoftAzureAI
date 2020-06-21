using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration
{
    public class TextAnalyticsServiceConfiguration : ITextAnalyticsServiceConfiguration
    {
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
    }

    public class TextAnalyticsServiceConfigurationValidation : IValidateOptions<TextAnalyticsServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, TextAnalyticsServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ApiKey)} configuration parameter for the Text Analytics service is required");
            }

            if (string.IsNullOrEmpty(options.Endpoint))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Endpoint)} configuration parameter for the Text Analytics service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
