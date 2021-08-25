using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.BLL.Validation;
using CardFile.DAL.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    public class CardFileService : ICardFileService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;

        public CardFileService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        public void Upload(int id, CardFileDTO model)
        {
            if (id == 0 || model == null)
                throw new CardFileException("Уou cannot upload a file.");


        }

        public void Download(string path, string fileName)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName))
                throw new CardFileException("Уou cannot add a card.");

        }

        public Task AddAsync(CardFileDTO model)
        {
            if (string.IsNullOrEmpty(model.FileName))
                throw new CardFileException("Уou cannot add a card. File Name is null or empty");
            if (string.IsNullOrEmpty(model.Path))
                throw new CardFileException("Уou cannot add a card. Path is null or empty");
            if (string.IsNullOrEmpty(model.Language))
                throw new CardFileException("Уou cannot add a card. Language is null or empty");
            if (string.IsNullOrEmpty(model.Description))
                throw new CardFileException("Уou cannot add a card. Description is null or empty");
            if (model.DateOfCreation > DateTime.Now || model.DateOfCreation < new DateTime(2021, 8, 23))
                throw new CardFileException("Уou cannot add a card. DateOfCreation is incorrect");

            var mappedCard = _mapper.Map<CardFileEntitie>(model);
       
            _unit.CardTextFileRepository.AddAsync(mappedCard);
            return _unit.SaveAsync();
        }

        public Task DeleteByIdAsync(int modelId)
        {
            if (modelId == 0)
                throw new CardFileException("Уou cannot delete a card. Card Id is null or empty");

            _unit.CardTextFileRepository.DeleteByIdAsync(modelId);
            return _unit.SaveAsync();
        }

        public IEnumerable<CardFileDTO> GetAll()
        {
            var cards = _unit.CardTextFileRepository.FindAll().ToList();

            return _mapper.Map<IEnumerable<CardFileDTO>>(cards);
        }

        public async Task<CardFileDTO> GetByIdAsync(int id)
        {
            var card = await _unit.CardTextFileRepository.GetByIdAsync(id);

            if (card == null)
                throw new CardFileException("Уou cannot get a card. Card Id is null or empty");

            return _mapper.Map<CardFileDTO>(card);
        }

        public IEnumerable<CardFileDTO> GetCardsByDateOfCreation(DateTime dateTime)
        {
            var cards = _unit.CardTextFileRepository.FindAll().Where(x => x.DateOfCreation == dateTime);

            if (cards == null)
                throw new CardFileException("Уou cannot get cards. Cards are null");

            return _mapper.Map<IEnumerable<CardFileDTO>>(cards);
        }

        public IEnumerable<CardFileDTO> GetCardsByLanguage(string language)
        {
            var cards = _unit.CardTextFileRepository.FindAll().Where(x => x.Language == language);

            if (cards == null)
                throw new CardFileException("Уou cannot get cards. Cards are null");

            return _mapper.Map<IEnumerable<CardFileDTO>>(cards);
        }

        public Task UpdateAsync(CardFileDTO model)
        {
            if (string.IsNullOrEmpty(model.FileName))
                throw new CardFileException("Уou cannot update a card. File Name is null or empty");
            if (string.IsNullOrEmpty(model.Path))
                throw new CardFileException("Уou cannot update a card. Path is null or empty");
            if (string.IsNullOrEmpty(model.Language))
                throw new CardFileException("Уou cannot update a card. Language is null or empty");
            if (string.IsNullOrEmpty(model.Description))
                throw new CardFileException("Уou cannot update a card. Description is null or empty");
            if (model.DateOfCreation > DateTime.Now || model.DateOfCreation < new DateTime(2021, 8, 23))
                throw new CardFileException("Уou cannot update a card. DateOfCreation is incorrect");

            var mappedCard = _mapper.Map<CardFileEntitie>(model);
      
            _unit.CardTextFileRepository.Update(mappedCard);
            return _unit.SaveAsync();
        }
    }
}