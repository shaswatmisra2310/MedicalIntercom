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
        
        string connectionforazure = "endpoint=https://commservicepoc.communication.azure.com/;accesskey=aW9DN0hy3PDGbum/fdGO05uL7SAJxQkFGRYDeLtxZiAmG9+LjVma/9fc0xD9bArpppZBRgj7EpV/OKzK5EvGIQ==";
        string endpointstring = "https://commservicepoc.communication.azure.com/";
        public IActionResult Index()
        {
            //var id = ViewBag.Identity;

            //var temp = HttpContext.User.Claims.First(c=>c.Type==ClaimTypes.Name);
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User user= db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();

            var AccessToken = GetAccessTokenChat(user);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            CurrentUserThreads currentUserThreads = new CurrentUserThreads();
            currentUserThreads.chatClient=chatClient;

            ViewBag.threads = GetThreads(currentUserThreads).ToString(); //pass to view and print with click options
            //ViewBag.threads = Listofthreads;
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
        //public AsyncPageable<ChatMessage> GetMessages(ChatClient chatClient, string threadId)

        //{
            
        //    ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: threadId);
        //    AsyncPageable<ChatMessage> allMessages = chatThreadClient.GetMessagesAsync();
        //    //await foreach (ChatMessage message in allMessages)
            
        //    return allMessages;
        //            //messages($"{message.Id}:{message.Content.Message}");
            
        //}
        //public async Task<SendChatMessageResult> SendMessage(ChatThreadClient chatThreadClient)
        //{
        //    SendChatMessageOptions sendChatMessageOptions = new SendChatMessageOptions()
        //    {
        //        Content = "This is the message",
        //        MessageType = ChatMessageType.Text
        //    };
        //    SendChatMessageResult sendChatMessageResult = await chatThreadClient.SendMessageAsync(sendChatMessageOptions);
        //    return sendChatMessageResult;
        //}

        //List All chat threads
        public List<CurrentUserThreads> GetThreads(CurrentUserThreads currentUserThreads)
        {

            var chatThreadItems =  currentUserThreads.chatClient.GetChatThreads().ToList();

            List<CurrentUserThreads> list = new List<CurrentUserThreads>();
            
            {
                CurrentUserThreads item = new CurrentUserThreads();
                {
                    item.chatClient = currentUserThreads.chatClient;
                }
                
                foreach (ChatThreadItem chatThreadItem in chatThreadItems)
                {
                    item.ChatTopics = chatThreadItem.Topic;
                    item.ChatThreadIds = chatThreadItem.Id;
                    item.chatClient = item.chatClient;
                    list.Add(item);   
                }
                
            }
            return list;
        }
        public IActionResult ChatThreads(IEnumerable<CurrentUserThreads> list)
        {
            
            return View();
        }
        //public string GetThread(ChatClient chatClient,ChatThreadClient chatThread)
        //{
        //    chatClient.GetChatThreadClient(chatThread.Id);
        //    return chatThread.Id;
        //}

    public string GetAccessTokenChat(User user)
        {
            var client = new CommunicationIdentityClient(connectionforazure);
            var userx = new CommunicationUserIdentifier(user.ChatIdentity);
            var tokenResponse = client.GetToken(userx, scopes: new[] { CommunicationTokenScope.Chat });
            var token = tokenResponse.Value.Token;
            var expiresOn = tokenResponse.Value.ExpiresOn;
            return token;
        }
        
        //public async Task<IActionResult> CreateNewChat(NewChatModel newchatmodel)
        //{
        //    var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
        //    var temp2 = temp;
        //    User user = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
        //    newchatmodel.user = user;
        //    newchatmodel.chattopic = "Chat Topic here";
        //    var AccessToken = GetAccessTokenChat(user);


        //    CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
        //    Uri endpoint = new Uri(endpointstring);
        //    ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
        //    newchatmodel.chatClient = chatClient;


        //    var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: newchatmodel.user.ChatIdentity))
        //    {
        //        DisplayName = newchatmodel.user.emailId
        //    };
        //    CreateChatThreadResult createChatThreadResult = await newchatmodel.chatClient.CreateChatThreadAsync(topic: newchatmodel.chattopic, participants: new[] { chatParticipant });
        //    return View();
        //    //RedirectToAction("AddAddUserToChat");
        //}
        //public IActionResult AddUserToChat(ChatParticipantModel chatparticipant)
        //{
        //    var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
        //    var temp2 = temp;
        //    User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
        //    var AccessToken = GetAccessTokenChat(userx);


        //    CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
        //    Uri endpoint = new Uri(endpointstring);
        //    ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
        //    chatparticipant.chatclient = chatClient;

        //    var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: chatparticipant.user.ChatIdentity))
        //    {
        //        DisplayName = chatparticipant.user.emailId
        //    };

        //    //ChatThreadResult = chatparticipant.chatclient.GetChatThreadClient();
        //    return View();
        //    }

        //public IActionResult NewChatPage()
        //{
        //    IEnumerable<User> users = db.UsersTable.Select(s => s).ToList();
        //    return View(users);
        //}
        //[HttpPost]
        //public IActionResult NewChatParticipants(ChatParticipantModel chatParticipantModel)
        //{


        //    var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: chatParticipantModel.ChatIdentity))
        //    {

        //        DisplayName = chatParticipantModel.EmailId
        //    };

        //    return RedirectToAction("NewChatPage");
        //}



    }
}
