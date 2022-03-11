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
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=UserDb");
        }

        public UserDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<User> UsersTable { get; set; }
        public DbSet<Role> Roles { get; set; }
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
                password = "123"

            });

        }

    }

}