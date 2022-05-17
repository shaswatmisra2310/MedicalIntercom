using Azure.Communication.Chat;

namespace MedicalIntercomProject.Models
{
    public class NewChatModel
    {
        public User[] User { get; set; }
        public string chattopic { get; set; }

        public ChatClient chatClient { get; set; }

        public string chatthreadId { get; set; }

        public User currentuser { get; set; }
    }
}
