using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using CardFile.BLL.Validation;
using CardFile.DAL.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CardFile.Identity.Extensions;

namespace CardFile.BLL.Services
{
    // TODO: при delete не видаляється файл в wwwroot, лише шлях в бд
    // TODO: при get, має бути можливість скачати документ
    public class CardFileService : ICardFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CardFileService(IUnitOfWork unit, 
                               IMapper mapper, 
                               IHostingEnvironment hostingEnvironment,
                               IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unit;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AddCardFileAsync(IFormFile uploadedFile, CardFileDTO cardFile)
        {
            if (uploadedFile == null)
                throw new CardFileException("Уou cannot add a file.");
            if (string.IsNullOrEmpty(cardFile.Language))
                throw new CardFileException("Уou cannot add a card. Language is null or empty");
            if (string.IsNullOrEmpty(cardFile.Description))
                throw new CardFileException("Уou cannot add a card. Description is null or empty");

            string path = "/Files/" + uploadedFile.FileName;

            using (var stream = File.Create(_hostingEnvironment.WebRootPath + path))
            {
                uploadedFile.CopyTo(stream);
            }

            cardFile.FileName = uploadedFile.FileName;
            cardFile.Path = path;
            cardFile.DateOfCreation = DateTime.Now;
            cardFile.UserId = _httpContextAccessor.GetUserId();
            
            var mappedFile = _mapper.Map<CardFileEntitie>(cardFile);

            await _unitOfWork.CardTextFileRepository.AddAsync(mappedFile);
            var added = await _unitOfWork.SaveAsync();
            return added > 0;
        }

        public async Task<bool> UpdateCardFileAsync(int cardFileId, IFormFile uploadedFile, CardFileDTO cardFile)
        {
            var userOwnsCardFile = await UserOwnsCardFileAsync(cardFileId, _httpContextAccessor.GetUserId());

            if (!userOwnsCardFile)
                throw new CardFileException("Уou do not own this card file.");

            if (uploadedFile == null)
                throw new CardFileException("Уou cannot add a file.");

            var  mappedFile = await _unitOfWork.CardTextFileRepository.GetByIdAsync(cardFileId);

            string path = "/Files/" + uploadedFile.FileName;

            using (var stream = File.Create(_hostingEnvironment.WebRootPath + path))
            {
                uploadedFile.CopyTo(stream);
            }

            mappedFile.FileName = uploadedFile.FileName;
            mappedFile.Path = path;
            mappedFile.Description = cardFile.Description;
            mappedFile.Language = cardFile.Language;

            _unitOfWork.CardTextFileRepository.Update(mappedFile);
            var updated = await _unitOfWork.SaveAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteByIdAsync(int cardFileId)
        {
            if (cardFileId == 0)
                throw new CardFileException("Уou cannot delete a card. Card Id is null or empty");

            var userOwnsCardFile = await UserOwnsCardFileAsync(cardFileId, _httpContextAccessor.GetUserId());

            if (!userOwnsCardFile)
                throw new CardFileException("Уou do not own this card file.");

            await _unitOfWork.CardTextFileRepository.DeleteByIdAsync(cardFileId);
            var deleted = await _unitOfWork.SaveAsync();
            return deleted > 0;
        }

        public async Task<bool> UserOwnsCardFileAsync(int cardFileId, string userId)
        {
            var cardFiles = await Task.Run(() => 
                _unitOfWork.CardTextFileRepository.FindAll().SingleOrDefault(x => x.Id == cardFileId));

            return cardFiles == null || cardFiles.UserId != userId
                ? false
                : true;
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