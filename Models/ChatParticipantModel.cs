﻿namespace MedicalIntercomProject.Models
{
    public class ChatParticipantModel
    {
        public int Id { get; set; }

        public string EmailId { get; set; }

        public String ChatIdentity { get; set; }

        public ChatParticipantModel()
        {
            Id = 0;
            EmailId = "someemailid";
            ChatIdentity = "chatidentityfromportal";

        }
    }

   
}
