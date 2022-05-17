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
        UserDbContext db;
        //context = new UserDbContext();
        public ChatController(UserDbContext _db)
        {
            db = _db;
        }
        //UserDbContext db = new UserDbContext();
        
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

            ViewBag.threads = GetThreads(currentUserThreads); //pass to view and print with click options
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
        
        public IActionResult GetMessages( string chatthreadId)

        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User user = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();

            var AccessToken = GetAccessTokenChat(user);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
           
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: chatthreadId);
            Message message = new Message();
            Pageable<ChatMessage> allMessages = chatThreadClient.GetMessages();
            List<Message> listx = new List<Message>();
            foreach (ChatMessage messages in allMessages)
            {
                
                message.MessageId = messages.Id;
                message.chatmessage = messages.Content.Message;
                //message.chatmessage = "trial";
                message.chatthreadId=chatthreadId;
                message.senderDisplayName = messages.SenderDisplayName;
                listx.Add(message);
                //listx.Add(message);
            }
            ViewBag.varlist=listx;
            return View();
                       

            }
        
        public IActionResult SendMessage(string message, string chatthreadId)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User user = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();

            var AccessToken = GetAccessTokenChat(user);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            //SendChatMessageOptions sendChatMessageOptions = new SendChatMessageOptions()
            //{
            //    Content = message,
            //    MessageType = ChatMessageType.Text
            //};
            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: chatthreadId);
            //SendChatMessageResult sendChatMessageResult = await chatThreadClient.SendMessageAsync(sendChatMessageOptions);
            SendChatMessageResult sendChatMessageResult = chatThreadClient.SendMessage(message,ChatMessageType.Text,user.emailId);

            //chatThreadClient.UpdateMessage(sendChatMessageResult.Id, message);
            return RedirectToAction("GetMessages",new RouteValueDictionary(new {Controller="Chat", Action="GetMessages", chatThreadId=chatthreadId}));
        }

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

        public async Task<IActionResult> CreateNewChat()
        {
           
            return View();
            
        }
        
        
        public IActionResult startchat(NewChatModel newchatmodel, string chattopic)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User user = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            newchatmodel.currentuser = user;
            newchatmodel.chattopic = chattopic;
            var AccessToken = GetAccessTokenChat(user);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            newchatmodel.chatClient = chatClient;


            var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: newchatmodel.currentuser.ChatIdentity))
            {
                DisplayName = newchatmodel.currentuser.emailId
            };

            CreateChatThreadResult createChatThreadResult = newchatmodel.chatClient.CreateChatThread(topic: newchatmodel.chattopic, participants: new[] { chatParticipant });

            newchatmodel.chattopic = createChatThreadResult.ChatThread.Topic;
            newchatmodel.chatthreadId = createChatThreadResult.ChatThread.Id;

            ViewBag.newchatmodel=newchatmodel;

            return RedirectToAction("AddUserToChat","Chat", new RouteValueDictionary(new { Controller = "Chat", Action = "AddUserToChat", NewChatModel=newchatmodel }));
        }
        public IActionResult AddUserToChat(NewChatModel newchatmodel)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            //chatparticipant.chatclient = chatClient;

            //var chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: chatparticipant.user.ChatIdentity))
            //{
            //    DisplayName = chatparticipant.user.emailId
            //};

            //ChatThreadResult = chatparticipant.chatclient.GetChatThreadClient();
            IEnumerable<User> users = db.UsersTable.Select(s => s).ToList();
            return View(users);
        }
        public IActionResult Add(int id, NewChatModel newchatmodel)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);

            //NewChatModel newchatmodel = ViewBag.newchatmodel;

            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            newchatmodel.chatClient = chatClient;
            User user = db.UsersTable.SingleOrDefault(s => s.Id == id);

            var threadid = newchatmodel.chatthreadId;
            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: threadid);
            ChatParticipant chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: user.ChatIdentity))
                 {

                DisplayName = user.emailId
           };
            chatThreadClient.AddParticipant(chatParticipant);


            return RedirectToAction("AddUserToChat", "Chat", new RouteValueDictionary(new { Controller = "Chat", Action = "AddUserToChat", NewChatModel = newchatmodel }));
        }

        //public IActionResult NewChatPage()
        //{
        //   
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
         
        
        public void DeleteChatThread(string threadId)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);

            //NewChatModel newchatmodel = ViewBag.newchatmodel;

            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            chatClient.DeleteChatThread(threadId);
        }


    }
}
