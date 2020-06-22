namespace AzureAI.CallCenterTalksAnalysis.Infrastructure.Utils
{
    public class ErrorMessage
    {
        public string Title { get; }
        public string Message { get; }


        public ErrorMessage(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
