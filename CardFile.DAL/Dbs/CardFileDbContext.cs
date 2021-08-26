﻿using CardFile.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardFile.DAL
{
    public class CardFileDbContext : DbContext
    {
        public DbSet<CardFileEntitie> CardTextFiles { get; set; }

        //public DbSet<RefreshToken> RefreshTokens { get; set; }

        public CardFileDbContext(DbContextOptions<CardFileDbContext> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CardFileAPI;Trusted_Connection=True;");
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}