namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces
{
    public interface IApplicationInsightsServiceConfiguration
    {
        string InstrumentationKey { get; set; }
    }
}
