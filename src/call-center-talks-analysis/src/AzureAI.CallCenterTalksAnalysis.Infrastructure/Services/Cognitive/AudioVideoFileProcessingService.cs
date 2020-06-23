using AzureAI.CallCenterTalksAnalysis.Core.Model;
using AzureAI.CallCenterTalksAnalysis.Core.Services.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.DTOs;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Storage.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive
{
    public class AudioVideoFileProcessingService : IAudioVideoFileProcessingService
    {
        private readonly IVideoIndexerServiceConfiguration _videoIndexerServiceConfiguration;
        private readonly IStorageService _storageService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<AudioVideoFileProcessingService> _log;

        public AudioVideoFileProcessingService(IVideoIndexerServiceConfiguration videoIndexerServiceConfiguration,
                                               IStorageService storageService,
                                               HttpClient httpClient,
                                               ILogger<AudioVideoFileProcessingService> log)
        {
            _videoIndexerServiceConfiguration = videoIndexerServiceConfiguration
                        ?? throw new ArgumentNullException(nameof(videoIndexerServiceConfiguration));
            _storageService = storageService
                        ?? throw new ArgumentNullException(nameof(storageService));
            _httpClient = httpClient
                        ?? throw new ArgumentNullException(nameof(httpClient));
            _log = log
                        ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<FileAnalysisResult> AnalyzeFileContentAsync(InputFileData inputFileData)
        {
            FileAnalysisResult fileAnalysisResult = new FileAnalysisResult();
            var sasToken = _storageService.GenerateSasToken();
            var videoUrl = $"{inputFileData.FilePath}?{sasToken}";

            AddApiKeyHeader();

            var accountAccessToken = await GetVideoIndexerAccountAccessTokenAsync();

            var content = new MultipartFormDataContent();
            var queryParams = CreateQueryString(
            new Dictionary<string, string>()
             {
                    {"accessToken", accountAccessToken},
                    {"name", $"new-video-{Guid.NewGuid()}"},
                    {"privacy", "private"},
                    {"partition", "partition"},
                    {"videoUrl", videoUrl},
             });

            var uploadRequestResult = await _httpClient.PostAsync($"{_videoIndexerServiceConfiguration.Endpoint}/{_videoIndexerServiceConfiguration.Location}/Accounts/{_videoIndexerServiceConfiguration.AccountId}/Videos?{queryParams}", content);
            var uploadResult = await uploadRequestResult.Content.ReadAsStringAsync();
            var videoId = JsonConvert.DeserializeObject<dynamic>(uploadResult)["id"];

            var videoTokenRequestResult = await _httpClient.GetAsync($"{_videoIndexerServiceConfiguration.Endpoint}/auth/" +
                $"{_videoIndexerServiceConfiguration.Location}/Accounts/" +
                $"{_videoIndexerServiceConfiguration.AccountId}/Videos/{videoId}/AccessToken?allowEdit=true");

            var videoAccessToken = await videoTokenRequestResult.Content.ReadAsStringAsync();
            videoAccessToken = videoAccessToken.Replace("\"", "");

            RemoveApiKeyHeader();

            queryParams = CreateQueryString(
            new Dictionary<string, string>()
             {
                    {"accessToken", videoAccessToken},
                    {"language", "English"}
             });

            AudioVideoAnalysisResult audioVideoAnalysisResult = null;
            while (true)
            {
                await Task.Delay(10000);

                var videoGetIndexRequestResult = await _httpClient.GetAsync($"{_videoIndexerServiceConfiguration.Endpoint}/{_videoIndexerServiceConfiguration.Location}/Accounts/" +
                    $"{_videoIndexerServiceConfiguration.AccountId}/Videos/{videoId}/Index?{queryParams}");
                var videoGetIndexResult = await videoGetIndexRequestResult.Content.ReadAsStringAsync();

                var processingState = JsonConvert.DeserializeObject<dynamic>(videoGetIndexResult)["state"];

                if (processingState != "Uploaded" && processingState != "Processing")
                {
                    audioVideoAnalysisResult = JsonConvert.DeserializeObject<AudioVideoAnalysisResult>(videoGetIndexResult);
                    break;
                }
            }

            if (audioVideoAnalysisResult != null)
            {
                if (audioVideoAnalysisResult.summarizedInsights != null)
                {
                    foreach (var sentimentValue in audioVideoAnalysisResult.summarizedInsights.sentiments)
                    {
                        fileAnalysisResult.SentimentValues.Add(sentimentValue.sentimentKey);
                    }
                }
            }

            return fileAnalysisResult;
        }

        private void AddApiKeyHeader()
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _videoIndexerServiceConfiguration.ApiKey);
            }
        }

        private void RemoveApiKeyHeader()
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Ocp-Apim-Subscription-Key"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");
            }
        }

        private async Task<string> GetVideoIndexerAccountAccessTokenAsync()
        {
            var accountAccessTokenRequestResult = await _httpClient.GetAsync($"{_videoIndexerServiceConfiguration.Endpoint}/auth/" +
                $"{_videoIndexerServiceConfiguration.Location}/Accounts/" +
                $"{_videoIndexerServiceConfiguration.AccountId}/AccessToken?allowEdit=true");
            var accountAccessToken = await accountAccessTokenRequestResult.Content.ReadAsStringAsync();
            accountAccessToken = accountAccessToken.Replace("\"", "");
            return accountAccessToken;
        }

        private string CreateQueryString(IDictionary<string, string> parameters)
        {
            var queryParameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in parameters)
            {
                queryParameters[parameter.Key] = parameter.Value;
            }

            return queryParameters.ToString();
        }
    }
}
