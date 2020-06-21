using System;
using System.Collections.Generic;

namespace AzureAI.CallCenterTalksAnalysis.Core.Model
{
    public class FileAnalysisResult : IEntity
    {
        public Guid Id { get; }
        public IList<string> SentimentValues { get; set; }

        public FileAnalysisResult()
        {
            Id = Guid.NewGuid();
            SentimentValues = new List<string>();
        }
    }
}
