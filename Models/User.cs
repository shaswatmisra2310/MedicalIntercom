namespace MedicalIntercomProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; } 
        
        public string? LastName { get; set; }

        public int RoleId { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public string emailId { get; set; }

        public string password { get; set; }

        public string ChatIdentity { get; set; }

        public Role Role { get; set; }

    }
}
