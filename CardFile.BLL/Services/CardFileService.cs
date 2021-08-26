using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.BLL.Validation;
using CardFile.DAL.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    public class CardFileService : ICardFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CardFileService(IUnitOfWork unit, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unit;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        public Task AddCardFileAsync(IFormFile uploadedFile, CardFileDTO cardFile)
        {
            if (uploadedFile == null)
                throw new CardFileException("Уou cannot add a file.");

            string path = "/Files/" + uploadedFile.FileName;

            using (var stream = File.Create(_hostingEnvironment.WebRootPath + path))
            {
                uploadedFile.CopyTo(stream);
            }

            cardFile.FileName = uploadedFile.FileName;
            cardFile.Path = path;
            cardFile.DateOfCreation = DateTime.Now;

            if (string.IsNullOrEmpty(cardFile.FileName))
                throw new CardFileException("Уou cannot add a card. File Name is null or empty");
            if (string.IsNullOrEmpty(cardFile.Path))
                throw new CardFileException("Уou cannot add a card. Path is null or empty");
            if (cardFile.DateOfCreation > DateTime.Now || cardFile.DateOfCreation < new DateTime(2021, 8, 23))
                throw new CardFileException("Уou cannot add a card. DateOfCreation is incorrect");
            if (string.IsNullOrEmpty(cardFile.Language))
                throw new CardFileException("Уou cannot add a card. Language is null or empty");
            if (string.IsNullOrEmpty(cardFile.Description))
                throw new CardFileException("Уou cannot add a card. Description is null or empty");

            var mappedFile = _mapper.Map<CardFileEntitie>(cardFile);

            _unitOfWork.CardTextFileRepository.AddAsync(mappedFile);
            return _unitOfWork.SaveAsync();
        }
              
        public Task DeleteByIdAsync(int modelId)
        {
            if (modelId == 0)
                throw new CardFileException("Уou cannot delete a card. Card Id is null or empty");

            _unitOfWork.CardTextFileRepository.DeleteByIdAsync(modelId);
            return _unitOfWork.SaveAsync();
        }

        public IEnumerable<CardFileDTO> GetAll()
        {
            var cards = _unitOfWork.CardTextFileRepository.FindAll().ToList();

            return _mapper.Map<IEnumerable<CardFileDTO>>(cards);
        }

        public async Task<CardFileDTO> GetByIdAsync(int id)
        {
            var card = await _unitOfWork.CardTextFileRepository.GetByIdAsync(id);

            if (card == null)
                throw new CardFileException("Уou cannot get a card. Card Id is null or empty");

            return _mapper.Map<CardFileDTO>(card);
        }

        public IEnumerable<CardFileDTO> GetCardsByDateOfCreation(DateTime dateTime)
        {
            var cards = _unitOfWork.CardTextFileRepository.FindAll().Where(x => x.DateOfCreation == dateTime);

            if (cards == null)
                throw new CardFileException("Уou cannot get cards. Cards are null");

            return _mapper.Map<IEnumerable<CardFileDTO>>(cards);
        }

        public IEnumerable<CardFileDTO> GetCardsByLanguage(string language)
        {
            var cards = _unitOfWork.CardTextFileRepository.FindAll().Where(x => x.Language == language);

            if (cards == null)
                throw new CardFileException("Уou cannot get cards. Cards are null");

            return _mapper.Map<IEnumerable<CardFileDTO>>(cards);
        }       
    }
}