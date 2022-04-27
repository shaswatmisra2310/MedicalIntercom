using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedicalIntercomProject.Models
{
    public class LoginViewModel
    {   
       public string EmailID { get; set; }
       
       public string password { get; set; }

        public LoginViewModel()
        {
            EmailID = "emailidhere";

            password = "passwordhere";
        }
       
    }

    
}
