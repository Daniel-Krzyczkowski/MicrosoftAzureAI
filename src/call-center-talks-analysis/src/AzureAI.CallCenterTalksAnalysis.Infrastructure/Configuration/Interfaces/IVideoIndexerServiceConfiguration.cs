namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces
{
    public interface IVideoIndexerServiceConfiguration : ICognitiveServiceConfiguration
    {
        string AccountId { get; set; }
        string Location { get; set; }
    }
}
