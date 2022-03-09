namespace MedicalIntercomProject.Models
{
    public class Role
    {   
        public int ID { get; set; } 
        public string RoleName { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
