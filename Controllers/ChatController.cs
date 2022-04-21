using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using Azure.Communication.Identity;
using Azure.Communication.Chat;
using MedicalIntercomProject.Models;
using Azure.Communication;
using Azure;
using System.Security.Claims;

namespace MedicalIntercomProject.Controllers
{
    public class ChatController : Controller
    {
        UserDbContext db = new UserDbContext();
        
        string connectionforazure = "endpoint = https://chatcommunicationservices.communication.azure.com/;accesskey=S7oBb5x6Q4pTYgg10SZH3dXAFsrDYx1u9mxAWNjasuUkHAgtv0fse/zwkoJVmGHU7XjC0o6CcAs4ip8OpxYkFg==";
        string endpointstring = "https://chatcommunicationservices.communication.azure.com/";
        public IActionResult Index()
        {
            //var id = ViewBag.Identity;

            var temp = HttpContext.Session.GetString("currentUser");
            //var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name);
            var temp2 = temp;
            User user = db.UsersTable.Where(s => s.emailId == temp2).SingleOrDefault();

            var AccessToken = GetAccessTokenChat(user);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            /*List<String> Listofthreads = GetThreads(chatClient);*/ //pass to view and print with click options

            // async Task newchat(ChatClient chatClient, User user)
            //{
            //    //foreach (String idx in ids)
            //    var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: user.ChatIdentity))
            //    {
            //        DisplayName = user.emailId
            //    };

            //    CreateChatThreadResult createChatThreadResult = await chatClient.CreateChatThreadAsync(topic: "CHAT TOPIC HERE", participants: new[] { chatParticipant });
            //}
            


            return View();
        }
        public async Task<AsyncPageable<ChatMessage>> GetMessages(ChatClient chatClient, string threadId)

        {
            
            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: threadId);
            AsyncPageable<ChatMessage> allMessages = chatThreadClient.GetMessagesAsync();
            //await foreach (ChatMessage message in allMessages)
            
            return allMessages;
                    //messages($"{message.Id}:{message.Content.Message}");
            
        }
        public async Task<SendChatMessageResult> SendMessage(ChatThreadClient chatThreadClient)
        {
            SendChatMessageOptions sendChatMessageOptions = new SendChatMessageOptions()
            {
                Content = "This is the message",
                MessageType = ChatMessageType.Text
            };
            SendChatMessageResult sendChatMessageResult = await chatThreadClient.SendMessageAsync(sendChatMessageOptions);
            return sendChatMessageResult;
        }

        //List All chat threads
        public async Task<List<String>> GetThreads(ChatClient chatClient)
        {
            AsyncPageable<ChatThreadItem> chatThreadItems = chatClient.GetChatThreadsAsync();
            List<String> Listofchatthreadids =new List<String>();
            int count = 0;
            await foreach (ChatThreadItem chatThreadItem in chatThreadItems)
            {
                // Console.WriteLine($"{ chatThreadItem.Id}");
                //if (chatThreadItem.Id != null)
                
                    Listofchatthreadids.Insert(count, chatThreadItem.Id);
                    count++;
                
            }
            return Listofchatthreadids;
        }
    public string GetAccessTokenChat(User user)
        {
            var client = new CommunicationIdentityClient(connectionforazure);
            var userx = new CommunicationUserIdentifier(user.ChatIdentity);
            var tokenResponse = client.GetToken(userx, scopes: new[] { CommunicationTokenScope.Chat });
            var token = tokenResponse.Value.Token;
            var expiresOn = tokenResponse.Value.ExpiresOn;
            return token;
        }

        public async Task NewChat(ChatClient chatClient, User user)
        {
            
            var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: user.ChatIdentity))
            {
                DisplayName = user.emailId
            };

            CreateChatThreadResult createChatThreadResult = await chatClient.CreateChatThreadAsync(topic: "CHAT TOPIC HERE", participants: new[] { chatParticipant });
        }

        public IActionResult NewChatPage()
        {
            IEnumerable<User> users = db.UsersTable.Select(s => s).ToList();
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> NewChatParticipants(ChatParticipantModel chatParticipantModel)
        {


            var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: chatParticipantModel.ChatIdentity))
            {

                DisplayName = chatParticipantModel.EmailId
            };

            return RedirectToAction("NewChatPage");
        }



    }
}
