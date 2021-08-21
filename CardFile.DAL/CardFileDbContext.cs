using CardFile.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL
{
    public class CardFileDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public CardFileDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=IdintityCardFileAPI;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //    entity.HasOne(x => x.Order)
            //        .WithOne(x => x.User);
            //});
            base.OnModelCreating(modelBuilder);
        }
    }
}
