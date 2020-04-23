using AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Services.Document
{
    public class DocumentContentExtractor : IDocumentContentExtractor
    {
        public async Task<byte[]> DownloadDocumentAsync(string documentUrl)
        {
            using (var webClient = new WebClient())
            {
                byte[] documentBytes = await webClient.DownloadDataTaskAsync(new Uri(documentUrl));
                return documentBytes;
            }
        }
    }
}
