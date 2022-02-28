using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedicalIntercomProject.Models
{
    public class LoginViewModel
    {   
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
       
    }
}
