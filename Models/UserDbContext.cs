using MedicalIntercomProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalIntercomProject
{
    public class UserDbContext : DbContext
    {
        public UserDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        public UserDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<User> UsersTable { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<UserChatThreadMapping> Chatthreaduserstable { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(new Role
            {
                ID = 1,
                RoleName = "Admin"
            }, new Role
            {
                ID = 2,
                RoleName = "User"
            });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                RoleId = 1,
                emailId = "admin@mail.com",
                password = "123",
                ChatIdentity = "8:acs:55cdd872-42d3-4c8c-a242-f2191f9c8b94_00000010-838f-0201-f40f-343a0d0033b9"

            });

        }

    }

}