namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Configuration.Interfaces
{
    public interface IStorageServiceConfiguration
    {
        string Key { get; set; }
        string AccountName { get; set; }
        string ContainerName { get; set; }
        string ConnectionString { get; set; }
    }
}
