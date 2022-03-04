using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedicalIntercomProject.Models
{
    public class NewUserViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        [Required]

        public string Role { get; set; }

    }
}
