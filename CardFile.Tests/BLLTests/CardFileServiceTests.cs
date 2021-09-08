//using CardFile.BLL.DTO;
//using CardFile.BLL.Interfaces;
//using CardFile.BLL.Services;
//using CardFile.BLL.Validation;
//using CardFile.DAL.Entities;
//using Data.Interfaces;
//using Microsoft.AspNetCore.Hosting;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Xunit;

//namespace CardFile.Tests.BLLTests
//{
//    public class CardFileServiceTests
//    {
//        [Fact]
//        public void CardFileService_GetAll_ReturnsCardFileDTOs()
//        {
//            var expected = GetTestCardFileDTOs().ToList();
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            CancellationToken cts = new CancellationToken();
//            mockUnitOfWork.Setup(m => m.CardFileRepository.FindAll(cts))
//                          .Returns(GetTestCardFileEntities().AsQueryable);
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

//            var actual = cardFileService.GetAll(cts).ToList();

//            for (int i = 0; i < actual.Count; i++)
//            {
//                Assert.Equal(expected[i].Id, actual[i].Id);
//                Assert.Equal(expected[i].Path, actual[i].Path);
//                Assert.Equal(expected[i].Language, actual[i].Language);
//            }
//        }

//        private IEnumerable<CardFileDTO> GetTestCardFileDTOs()
//        {
//            return new List<CardFileDTO>()
//            {
//                new CardFileDTO
//                {
//                    Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                    Language = "sql", Description = "sql",UserId = "1"
//                },
//                new CardFileDTO
//                {
//                    Id = 2, FileName = "EF.txt", Path = "/Files/EF.txt", DateOfCreation = DateTime.Now,
//                    Language = "ef", Description = "ef", UserId = "2"
//                },
//                new CardFileDTO 
//                {
//                    Id = 3, FileName = "Git command.txt", Path = "/Files/Git command.txt", DateOfCreation = DateTime.Now,
//                    Language = "git", Description = "git", UserId = "3"
//                },
//                new CardFileDTO 
//                {
//                    Id = 4, FileName = "Angular.txt", Path = "/Files/Angular.txt", DateOfCreation = DateTime.Now,
//                    Language = "Angular", Description = "Angular", UserId = "4"
//                }
//            };
//        }

//        [Fact]
//        public async Task BookService_GetByIdAsync_ReturnsBookModel()
//        {
//            var expected = GetTestCardFileDTOs().First();
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(m => m.CardFileRepository.GetByIdAsync(It.IsAny<int>()))
//                          .ReturnsAsync(GetTestCardFileEntities().First);
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
//            var actual = await cardFileService.GetByIdAsync(1);

//            Assert.Equal(expected.Id, actual.Id);
//            Assert.Equal(expected.Path, actual.Path);
//            Assert.Equal(expected.Language, actual.Language);
//        }

//        private List<CardFileEntitie> GetTestCardFileEntities()
//        {
//            return new List<CardFileEntitie>()
//            {
//                new CardFileEntitie
//                {
//                    Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                    Language = "sql", Description = "sql",UserId = "1"
//                },
//                new CardFileEntitie
//                {
//                    Id = 2, FileName = "EF.txt", Path = "/Files/EF.txt", DateOfCreation = DateTime.Now,
//                    Language = "ef", Description = "ef", UserId = "2"
//                },
//                new CardFileEntitie 
//                {
//                    Id = 3, FileName = "Git command.txt", Path = "/Files/Git command.txt", DateOfCreation = DateTime.Now,
//                    Language = "git", Description = "git", UserId = "3"
//                },
//                new CardFileEntitie 
//                {
//                    Id = 4, FileName = "Angular.txt", Path = "/Files/Angular.txt", DateOfCreation = DateTime.Now,
//                    Language = "Angular", Description = "Angular", UserId = "4"
//                }
//            };
//        }

//        [Fact]
//        public async Task CardFileService_AddCardFileAsync_AddsCardFile()
//        {
//            //Arrange
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.AddAsync(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                Language = "sql", Description = "sql", UserId = "1"
//            };

//            //Act
//            await cardFileService.AddCardFileAsync(cardFile);

//            //Assert
//            mockUnitOfWork.Verify(x => x.CardFileRepository.AddAsync(It.Is<CardFileEntitie>(b => b.Language == cardFile.Language && b.Id == cardFile.Id)), Times.Once);
//            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
//        }

//        [Fact]
//        public void CardFileService_AddCardFileAsync_ThrowsCardFileExceptionWithWrongDateOfCreation()
//        {
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.AddAsync(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = new DateTime(2021,06,20),
//                Language = "sql", Description = "sql", UserId = "1"
//            };

//            Assert.ThrowsAsync<CardFileException>(() => cardFileService.AddCardFileAsync(cardFile));
//        }

//        [Fact]
//        public void CardFileService_AddCardFileAsync_ThrowsCardFileExceptionWithEmptyLanguage()
//        {
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.AddAsync(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                Language = "", Description = "sql", UserId = "1"
//            };

//            Assert.ThrowsAsync<CardFileException>(() => cardFileService.AddCardFileAsync(cardFile));
//        }

//        [Fact]
//        public void CardFileService_AddCardFileAsync_ThrowsCardFileExceptionWithEmptyDescription()
//        {
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.AddAsync(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                Language = "sql", Description = "", UserId = "1"
//            };

//            Assert.ThrowsAsync<CardFileException>(() => cardFileService.AddCardFileAsync(cardFile));
//        }

//        [Theory]
//        [InlineData(1)]
//        [InlineData(2)]
//        [InlineData(100)]
//        public async Task CardFileService_DeleteByIdAsync_DeletesCardFile(int cardFileId)
//        {
//            //Arrange
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.DeleteByIdAsync(It.IsAny<int>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

//            //Act
//            await cardFileService.DeleteByIdAsync(cardFileId);

//            //Assert
//            mockUnitOfWork.Verify(x => x.CardFileRepository.DeleteByIdAsync(cardFileId), Times.Once);
//            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
//        }

//        [Fact]
//        public async Task CardFileService_UpdateAsync_UpdatesCardFile()
//        {
//            //Arrange
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                Language = "sql", Description = "sql", UserId = "1"
//            };
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.Update(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

//            //Act
//            await cardFileService.UpdateCardFileAsync(cardFile);

//            //Assert
//            mockUnitOfWork.Verify(x => x.CardFileRepository.Update(It.Is<CardFileEntitie>(b => b.Language == cardFile.Language && b.Id == cardFile.Id)), Times.Once);
//            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
//        }

//        [Fact]
//        public void CardFileService_UpdateCardFileAsync_ThrowsCardFileExceptionWithEmptyDescription()
//        {
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                Language = "sql", Description = "", UserId = "1"
//            };
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.Update(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

//            Assert.ThrowsAsync<CardFileException>(() => cardFileService.UpdateCardFileAsync(cardFile));
//        }

//        [Fact]
//        public void CardFileService_UpdateCardFileAsync_ThrowsCardFileExceptionWithEmptyLanguage()
//        {
//            var cardFile = new CardFileDTO
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
//                Language = "", Description = "sql", UserId = "1"
//            };
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.Update(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

//            Assert.ThrowsAsync<CardFileException>(() => cardFileService.UpdateCardFileAsync(cardFile));
//        }

//        [Fact]
//        [Obsolete]
//        public void CardFileService_UpdateCardFileAsync_ThrowsCardFileExceptionWithWrongDateOfCreation()
//        {
//            var cardFile = new CardFileDTO 
//            {
//                Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = new DateTime(2021, 06, 20),
//                Language = "sql", Description = "sql", UserId = "1"
//            };
//            var mockUnitOfWork = new Mock<IUnitOfWork>();
//            mockUnitOfWork.Setup(x => x.CardFileRepository.Update(It.IsAny<CardFileEntitie>()));
//            ICardFileService cardFileService = new CardFileService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

//            Assert.ThrowsAsync<CardFileException>(() => cardFileService.UpdateCardFileAsync(cardFile.Id, cardFile));
//        }
//    }
//}
