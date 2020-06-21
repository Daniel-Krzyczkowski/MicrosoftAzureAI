namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces
{
    public interface ICognitiveServiceConfiguration
    {
        string ApiKey { get; set; }
        string Endpoint { get; set; }
    }
}
