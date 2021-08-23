using CardFile.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardFile.DAL
{
    public class CardFileDbContext : DbContext
    {
        public DbSet<CardTextFile> CardTextFiles { get; set; }

        //public DbSet<RefreshToken> RefreshTokens { get; set; }

        public CardFileDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CardFileAPI;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //    entity.HasOne(x => x.Order)
            //        .WithOne(x => x.User);
            //});

            //modelBuilder.Entity<Order>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //    entity.HasMany(x => x.OrderItems)
            //        .WithOne(x => x.Order);
            //});

            //modelBuilder.Entity<Product>(entity =>
            //{
            //    entity.HasOne(x => x.Category)
            //        .WithOne(x => x.Product);
            //    entity.HasKey(x => x.Id);
            //});

            //modelBuilder.Entity<ProductCategory>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //});

            //modelBuilder.Entity<OrderItem>(entity =>
            //{
            //    entity.HasKey(x => x.Id);
            //});
        }
    }
}