using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MedicalIntercomProject.Models
{
    [Keyless]
    public class NewUserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public string ChatId { get; set; }

        [Required]

        public string Role { get; set; }

    }
}
