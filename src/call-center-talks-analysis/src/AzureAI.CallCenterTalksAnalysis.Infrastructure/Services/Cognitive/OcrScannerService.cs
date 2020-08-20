using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Services.Cognitive
{
    public class OcrScannerService : IOcrScannerService
    {
        private readonly FormRecognizerClient _formRecognizerClient;
        private readonly ILogger<OcrScannerService> _log;

        public OcrScannerService(FormRecognizerClient formRecognizerClient,
                                 ILogger<OcrScannerService> log)
        {
            _formRecognizerClient = formRecognizerClient
                    ?? throw new ArgumentNullException(nameof(formRecognizerClient));
            _log = log
                    ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<string> ScanDocumentAndGetResultsAsync(string documentUrl)
        {
            FormPageCollection formPages = await _formRecognizerClient.StartRecognizeContentFromUriAsync(new Uri(documentUrl)).WaitForCompletionAsync();
            StringBuilder sb = new StringBuilder();

            foreach (FormPage page in formPages)
            {
                for (int i = 0; i < page.Lines.Count; i++)
                {
                    FormLine line = page.Lines[i];
                    sb.AppendLine(line.Text);
                }
            }

            return sb.ToString();
        }
    }
}
