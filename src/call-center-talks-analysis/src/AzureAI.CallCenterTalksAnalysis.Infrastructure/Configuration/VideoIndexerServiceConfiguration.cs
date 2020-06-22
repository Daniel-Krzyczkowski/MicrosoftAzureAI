using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration
{
    public class VideoIndexerServiceConfiguration : IVideoIndexerServiceConfiguration
    {
        public string ApiKey { get; set; }
        public string Endpoint { get; set; }
        public string AccountId { get; set; }
        public string Location { get; set; }
    }

    public class VideoIndexerServiceConfigurationValidation : IValidateOptions<VideoIndexerServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, VideoIndexerServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ApiKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ApiKey)} configuration parameter for the Video Indexer service is required");
            }

            if (string.IsNullOrEmpty(options.Endpoint))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Endpoint)} configuration parameter for the Video Indexer service is required");
            }

            if (string.IsNullOrEmpty(options.AccountId))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.AccountId)} configuration parameter for the Video Indexer service is required");
            }

            if (string.IsNullOrEmpty(options.Location))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Location)} configuration parameter for the Video Indexer service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
