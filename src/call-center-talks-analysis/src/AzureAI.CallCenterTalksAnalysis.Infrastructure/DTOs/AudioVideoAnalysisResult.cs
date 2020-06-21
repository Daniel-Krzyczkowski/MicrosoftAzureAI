using System.Collections.Generic;

namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.DTOs
{
    internal class AudioVideoAnalysisResult
    {
        public SummarizedInsights summarizedInsights { get; set; }
    }

    internal class SummarizedInsights
    {
        public IList<Keyword> keywords { get; set; }
        public IList<Sentiment> sentiments { get; set; }
        public IList<Emotion> emotions { get; set; }
        public IList<Topic> topics { get; set; }
    }

    internal class Instance
    {
        public string adjustedStart { get; set; }
        public string adjustedEnd { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }

    internal class Keyword
    {
        public int id { get; set; }
        public string text { get; set; }
        public double confidence { get; set; }
        public string language { get; set; }
        public IList<Instance> instances { get; set; }
    }

    internal class Topic
    {
        public int id { get; set; }
        public string name { get; set; }
        public string referenceId { get; set; }
        public string referenceType { get; set; }
        public string iptcName { get; set; }
        public double confidence { get; set; }
        public string iabName { get; set; }
        public string language { get; set; }
        public IList<Instance> instances { get; set; }
    }

    internal class Sentiment
    {
        public int id { get; set; }
        public double averageScore { get; set; }
        public string sentimentKey { get; set; }
        public IList<Instance> instances { get; set; }
    }

    public class Emotion
    {
        public string type { get; set; }
        public double seenDurationRatio { get; set; }
    }
}
