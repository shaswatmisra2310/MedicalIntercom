﻿using Azure.Communication.Chat;

namespace MedicalIntercomProject.Models
{
    public class ChatParticipantModel
    {
        

        public ChatClient chatclient { get; set; }

        public User user { get; set; }
    }
}
