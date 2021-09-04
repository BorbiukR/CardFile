using CardFile.DAL.Entities;
using CardFile.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CardFile.DAL
{
    public class CardFileDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<CardFileEntitie> CardFiles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public CardFileDbContext(DbContextOptions<CardFileDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}