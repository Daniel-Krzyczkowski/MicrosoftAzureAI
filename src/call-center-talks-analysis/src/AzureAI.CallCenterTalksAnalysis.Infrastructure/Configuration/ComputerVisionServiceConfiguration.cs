using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration
{
    public class ComputerVisionServiceConfiguration : IComputerVisionServiceConfiguration
    {
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
    }

    public class ComputerVisionServiceConfigurationValidation : IValidateOptions<ComputerVisionServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, ComputerVisionServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ApiKey)} configuration parameter for the Computer Vision service is required");
            }

            if (string.IsNullOrEmpty(options.Endpoint))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Endpoint)} configuration parameter for the Computer Vision service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
