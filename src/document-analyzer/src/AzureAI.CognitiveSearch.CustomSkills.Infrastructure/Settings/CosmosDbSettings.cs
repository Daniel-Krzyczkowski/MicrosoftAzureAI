namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Settings
{
    public class CosmosDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public string PartitionKeyPath { get; set; }
    }
}
