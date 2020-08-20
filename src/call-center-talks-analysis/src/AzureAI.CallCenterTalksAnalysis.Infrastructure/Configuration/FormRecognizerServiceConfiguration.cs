using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration
{
    public class FormRecognizerServiceConfiguration : IFormRecognizerServiceConfiguration
    {
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
    }

    public class FormRecognizerServiceConfigurationValidation : IValidateOptions<FormRecognizerServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, FormRecognizerServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ApiKey)} configuration parameter for the Form Recognizer service is required");
            }

            if (string.IsNullOrEmpty(options.Endpoint))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Endpoint)} configuration parameter for the Form Recognizer service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
