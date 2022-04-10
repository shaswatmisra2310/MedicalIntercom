using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using Azure.Communication.Identity;
using Azure.Communication.Chat;
using MedicalIntercomProject.Models;
using Azure.Communication;

namespace MedicalIntercomProject.Controllers
{
    public class ChatController : Controller
    {
        UserDbContext db;
        string connectionforazure = "endpoint = https://chatcommunicationservices.communication.azure.com/;accesskey=S7oBb5x6Q4pTYgg10SZH3dXAFsrDYx1u9mxAWNjasuUkHAgtv0fse/zwkoJVmGHU7XjC0o6CcAs4ip8OpxYkFg==";
        string endpointstring = "https://chatcommunicationservices.communication.azure.com/";
        public IActionResult Index()
        {
            //var id = ViewBag.Identity;
            //User user = db.UsersTable.Where(s => s.ChatIdentity == id).SingleOrDefault();

            //var AccessToken = GetAccessTokenChat(user);


            //CommunicationTokenCredential communicationTokenCredential = new CommunicationTokenCredential(AccessToken);
            //Uri endpoint = new Uri(endpointstring);
            //ChatClient chatClient = new ChatClient(endpoint, communicationTokenCredential);



            return View();
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



    }
}
