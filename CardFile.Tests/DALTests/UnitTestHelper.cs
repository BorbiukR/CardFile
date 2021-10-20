//using System;
//using AutoMapper;
//using CardFile.BLL.MappingProfiles;
//using CardFile.DAL;
//using CardFile.DAL.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace CardFile.Tests
//{
//    internal static class UnitTestHelper
//    {
//        public static DbContextOptions<CardFileDbContext> GetXUnitTestDbOptions()
//        {
//            var options = new DbContextOptionsBuilder<CardFileDbContext>()
//                .UseInMemoryDatabase(Guid.NewGuid().ToString())
//                .Options;

//            using (var context = new CardFileDbContext(options))
//            {
//                SeedData(context);
//            }

//            return options;
//        }

//        public static void SeedData(CardFileDbContext context)
//        {
//            context.CardFiles.Add(new CardFileEntitie 
//            {
//                Id = 1, 
//                FileName = "SQL.txt",
//                Path = "/Files/SQL.txt",
//                DateOfCreation = DateTime.Now,
//                Language = "sql",
//                Description = "sql", 
//                UserId = "1"
//            });
//            context.CardFiles.Add(new CardFileEntitie
//            {
//                Id = 2,
//                FileName = "EF.txt",
//                Path = "/Files/EF.txt",
//                DateOfCreation = DateTime.Now,
//                Language = "ef",
//                Description = "ef",
//                UserId = "2"
//            });
//            context.CardFiles.Add(new CardFileEntitie
//            {
//                Id = 3,
//                FileName = "Git command.txt",
//                Path = "/Files/Git command.txt",
//                DateOfCreation = DateTime.Now,
//                Language = "git",
//                Description = "git",
//                UserId = "3"
//            });

//            context.SaveChanges();
//        }

//        public static Mapper CreateMapperProfile()
//        {
//            var myProfile = new BLLAutomapperProfile();
//            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

//            return new Mapper(configuration);
//        }
//    }
//}