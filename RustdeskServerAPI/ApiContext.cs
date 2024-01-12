using Microsoft.EntityFrameworkCore;
using RustdeskServerAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace RustdeskServerAPI
{
    public class ApiContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserPeers> UserPeers { get; set; } = null!;
        public DbSet<Peer> Peers { get; set; } = null!;

        public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                    new User { 
                        Id = 1,
                        UserId = "",
                        Name = "Admin Adminovich",
                        Email="some.email@.mail.com",
                        isAdmin = true,
                        Login = "Admin",
                        Password = string.Concat(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes("1")).Select(b=>b.ToString("X2"))),
                        Note = "Default admin user",
                        Status = 0,
                        Uuid = "",
                        Token = ""
                    }
            );
        }
    }
}
