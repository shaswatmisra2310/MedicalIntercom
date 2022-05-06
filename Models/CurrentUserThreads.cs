using Azure.Communication.Chat;

namespace MedicalIntercomProject.Models
{
    public class CurrentUserThreads
    {
        
        public string? ChatTopics { get; set; }
        public string? ChatThreadIds { get; set; }

        public ChatClient chatClient { get; set; }
    }
}
