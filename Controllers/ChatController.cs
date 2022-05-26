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
        public ChatController(UserDbContext _db)
        {
            db = _db;
        }

        string connectionforazure = "endpoint=https://commservicepoc.communication.azure.com/;accesskey=aW9DN0hy3PDGbum/fdGO05uL7SAJxQkFGRYDeLtxZiAmG9+LjVma/9fc0xD9bArpppZBRgj7EpV/OKzK5EvGIQ==";
        string endpointstring = "https://commservicepoc.communication.azure.com/";
        public IActionResult Index()
        {
            
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
            
            Pageable<ChatMessage> allMessages = chatThreadClient.GetMessages();
            List<Message> listx = new List<Message>();
            foreach (ChatMessage messages in allMessages)
            {
                if (messages.SenderDisplayName != "" & messages.Content.Message != "" & messages.SenderDisplayName != null & messages.Content.Message !=null & messages.DeletedOn == null)
                {
                    Message messagevar = new Message();
                    messagevar.MessageId = messages.Id;
                    messagevar.chatmessage = messages.Content.Message;
                    messagevar.chatthreadId = chatthreadId;
                    messagevar.senderDisplayName = messages.SenderDisplayName;
                    listx.Add(messagevar);
                }
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
            
            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: chatthreadId);
           
            SendChatMessageResult sendChatMessageResult = chatThreadClient.SendMessage(message,ChatMessageType.Text,user.emailId);

            return RedirectToAction("GetMessages",new RouteValueDictionary(new {Controller="Chat", Action="GetMessages", chatThreadId=chatthreadId}));
        }

        //List All chat threads
        public List<CurrentUserThreads> GetThreads(CurrentUserThreads currentUserThreads)
        {

            var chatThreadItems =  currentUserThreads.chatClient.GetChatThreads().ToList();

            List<CurrentUserThreads> list = new List<CurrentUserThreads>();
            
            {
            
                
                foreach (ChatThreadItem chatThreadItem in chatThreadItems)
                {
                    if (chatThreadItem.DeletedOn == null)
                    {
                        CurrentUserThreads item = new CurrentUserThreads();
                        item.chatClient = currentUserThreads.chatClient;
                        item.ChatTopics = chatThreadItem.Topic;
                        item.ChatThreadIds = chatThreadItem.Id;
                        item.chatClient = item.chatClient;
                        list.Add(item);
                    }
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

        public IActionResult CreateNewChat()
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

            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: createChatThreadResult.ChatThread.Id);

            SendChatMessageResult sendChatMessageResult = chatThreadClient.SendMessage("Chat Created", ChatMessageType.Text, "System");

            newchatmodel.chattopic = createChatThreadResult.ChatThread.Topic;
            newchatmodel.chatthreadId = createChatThreadResult.ChatThread.Id;

            ViewBag.chatthreadId=newchatmodel.chatthreadId;



            return RedirectToAction("AddUserToChat", "Chat", new RouteValueDictionary(new { Controller = "Chat", Action = "AddUserToChat", NewChatModel = newchatmodel,chatthreadid=newchatmodel.chatthreadId }));
        }
        public IActionResult AddUserToChat(NewChatModel newchatmodel,string chatthreadid)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);


            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            ViewBag.chatthreadid = chatthreadid;
            
            IEnumerable<User> users = db.UsersTable.Select(s => s).ToList();
            return View(users);
        }
        public IActionResult Add(string id,string chatthreadid)
        {
            AddUserToChat addUserToChat = new AddUserToChat();
            addUserToChat.id = id;
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);

            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            addUserToChat.chatClient = chatClient;
            User user = db.UsersTable.SingleOrDefault(s => s.ChatIdentity == addUserToChat.id);


            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: chatthreadid);
            ChatParticipant chatParticipant = new ChatParticipant(identifier: new CommunicationUserIdentifier(id: user.ChatIdentity))
                 {

                DisplayName = user.emailId
           };
            chatThreadClient.AddParticipant(chatParticipant);

            IEnumerable<User> users = db.UsersTable.Select(s => s).ToList();


            return RedirectToAction("AddUserToChat", "Chat", new RouteValueDictionary(new { Controller = "Chat", Action = "AddUserToChat", users ,chatthreadid}));
        }

       
         
        
        public IActionResult DeleteChatThread(string threadId)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);

            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            chatClient.DeleteChatThread(threadId);
            return RedirectToAction("Index", "Chat");
        }

        public IActionResult DeleteMessage(string messageid, string chatthreadId)
        {
            var temp = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email);
            var temp2 = temp;
            User userx = db.UsersTable.Where(s => s.emailId == temp2.Value).SingleOrDefault();
            var AccessToken = GetAccessTokenChat(userx);

            CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            Uri endpoint = new Uri(endpointstring);
            ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);
            ChatThreadClient chatThreadClient = chatClient.GetChatThreadClient(threadId: chatthreadId);

            chatThreadClient.DeleteMessage(messageid);



            return RedirectToAction("GetMessages", new RouteValueDictionary(new { Controller = "Chat", Action = "GetMessages", chatThreadId = chatthreadId }));
        }

        public IActionResult ChatCreated (string chatthreadId)
        {
            return RedirectToAction("GetMessages", new RouteValueDictionary(new { Controller = "Chat", Action = "GetMessages", chatThreadId = chatthreadId }));
        }


    }
}
