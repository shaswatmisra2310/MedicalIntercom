using MedicalIntercomProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalIntercomProject
{
    public class UserDbContext : DbContext
    {   
        public UserDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<User> UsersTable { get; set; }
        public DbSet<Role> Roles { get; set; }
        
    }
}
