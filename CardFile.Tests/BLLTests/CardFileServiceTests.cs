using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.MappingProfiles;
using CardFile.BLL.Services;
using CardFile.BLL.Validation;
using CardFile.DAL.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CardFile.Tests.BLLTests
{
    public class CardFileServiceTests : Profile
    {
        private readonly CardFileService _cardFileService;
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private readonly IMapper _mapper;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new Mock<IHttpContextAccessor>();
        private readonly Mock<IHostingEnvironment> _hostingEnvironment = new Mock<IHostingEnvironment>();
                            // IWebHostEnvironment 
        private readonly Mock<IFormFile> _formFile = new Mock<IFormFile>();
                          
        public CardFileServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new BLLAutomapperProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _cardFileService = new CardFileService(_unitOfWork.Object, 
                                       _mapper, 
                                       _hostingEnvironment.Object, 
                                       _httpContextAccessor.Object);
        }

        [Fact]
        public void CardFileService_GetAll_ReturnsCardFileDTOs()
        {
            CancellationToken cts = new CancellationToken();

            _unitOfWork.Setup(m => m.CardFileRepository.FindAll(cts))
                          .Returns(GetTestCardFileEntities().AsQueryable);

            var expected = GetTestCardFileDTOs().ToList();
            var actual = _cardFileService.GetAll(cts).ToList();

            for (int i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i].Id, actual[i].Id);
                Assert.Equal(expected[i].Path, actual[i].Path);
                Assert.Equal(expected[i].Language, actual[i].Language);
            }
        }

        [Fact]
        public async Task CardFileService_GetByIdAsync_ReturnCardFileDTO()
        {
            int cardFileId = 1;
            _unitOfWork.Setup(m => m.CardFileRepository.GetByIdAsync(It.IsAny<int>()))
                          .ReturnsAsync(GetTestCardFileEntities().First());

            var expected = GetTestCardFileDTOs().First();
            var actual = await _cardFileService.GetByIdAsync(cardFileId);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Path, actual.Path);
            Assert.Equal(expected.Language, actual.Language);
        }
     
        [Fact]
        public void CardFileService_AddCardFileAsync_ThrowsCardFileExceptionWithEmptyLanguage()
        {
            _unitOfWork.Setup(m => m.CardFileRepository.AddAsync(It.IsAny<CardFileEntitie>()));
      
            var cardFile = new CardFileDTO
            {
                Id = 1,
                FileName = "SQL.txt",
                Path = "/Files/SQL.txt",
                DateOfCreation = new DateTime(2021, 06, 20),
                Language = "sql",
                Description = "sql",
                UserId = "1"
            };

            Assert.ThrowsAsync<CardFileException>(() 
                => _cardFileService.AddCardFileAsync(_formFile.Object, cardFile));
        }

        [Fact]
        public void CardFileService_AddCardFileAsync_ThrowsCardFileExceptionWithEmptyDescription()
        {
            _unitOfWork.Setup(m => m.CardFileRepository.AddAsync(It.IsAny<CardFileEntitie>()));

            var cardFile = new CardFileDTO
            {
                Id = 1,
                FileName = "SQL.txt",
                Path = "/Files/SQL.txt",
                DateOfCreation = new DateTime(2021, 06, 20),
                Language = "",
                Description = "",
                UserId = "1"
            };

            Assert.ThrowsAsync<CardFileException>(()
                => _cardFileService.AddCardFileAsync(_formFile.Object, cardFile));
        }
      
        [Fact]
        public void CardFileService_UpdateCardFileAsync_ThrowsCardFileExceptionWithEmptyDescription()
        {
            _unitOfWork.Setup(m => m.CardFileRepository.Update(It.IsAny<CardFileEntitie>()));

            var cardFile = new CardFileDTO
            {
                Id = 1,
                FileName = "SQL.txt",
                Path = "/Files/SQL.txt",
                DateOfCreation = DateTime.Now,
                Language = "sql",
                Description = "",
                UserId = "1"
            };

            Assert.ThrowsAsync<CardFileException>(() 
                => _cardFileService.UpdateCardFileAsync(cardFile.Id, _formFile.Object, cardFile));
        }

        [Fact]
        public void CardFileService_UpdateCardFileAsync_ThrowsCardFileExceptionWithEmptyLanguage()
        {
            _unitOfWork.Setup(m => m.CardFileRepository.Update(It.IsAny<CardFileEntitie>()));

            var cardFile = new CardFileDTO
            {
                Id = 1,
                FileName = "SQL.txt",
                Path = "/Files/SQL.txt",
                DateOfCreation = DateTime.Now,
                Language = "",
                Description = "sql",
                UserId = "1"
            };

            Assert.ThrowsAsync<CardFileException>(()
                => _cardFileService.UpdateCardFileAsync(cardFile.Id, _formFile.Object, cardFile));
        }

        private IEnumerable<CardFileDTO> GetTestCardFileDTOs()
        {
            return new List<CardFileDTO>()
            {
                new CardFileDTO
                {
                    Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
                    Language = "sql", Description = "sql",UserId = "1"
                },
                new CardFileDTO
                {
                    Id = 2, FileName = "EF.txt", Path = "/Files/EF.txt", DateOfCreation = DateTime.Now,
                    Language = "ef", Description = "ef", UserId = "2"
                },
                new CardFileDTO
                {
                    Id = 3, FileName = "Git command.txt", Path = "/Files/Git command.txt", DateOfCreation = DateTime.Now,
                    Language = "git", Description = "git", UserId = "3"
                },
                new CardFileDTO
                {
                    Id = 4, FileName = "Angular.txt", Path = "/Files/Angular.txt", DateOfCreation = DateTime.Now,
                    Language = "Angular", Description = "Angular", UserId = "4"
                }
            };
        }

        private List<CardFileEntitie> GetTestCardFileEntities()
        {
            return new List<CardFileEntitie>()
            {
                new CardFileEntitie
                {
                    Id = 1, FileName = "SQL.txt", Path = "/Files/SQL.txt", DateOfCreation = DateTime.Now,
                    Language = "sql", Description = "sql", UserId = "1"
                },
                new CardFileEntitie
                {
                    Id = 2, FileName = "EF.txt", Path = "/Files/EF.txt", DateOfCreation = DateTime.Now,
                    Language = "ef", Description = "ef", UserId = "2"
                },
                new CardFileEntitie
                {
                    Id = 3, FileName = "Git command.txt", Path = "/Files/Git command.txt", DateOfCreation = DateTime.Now,
                    Language = "git", Description = "git", UserId = "3"
                },
                new CardFileEntitie
                {
                    Id = 4, FileName = "Angular.txt", Path = "/Files/Angular.txt", DateOfCreation = DateTime.Now,
                    Language = "Angular", Description = "Angular", UserId = "4"
                }
            };
        }
    }
}