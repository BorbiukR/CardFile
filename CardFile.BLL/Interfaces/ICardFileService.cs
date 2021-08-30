using CardFile.BLL.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface ICardFileService : ICrud<CardFileDTO>
    {
        IEnumerable<CardFileDTO> GetCardsByDateOfCreation(DateTime dateTime);

        IEnumerable<CardFileDTO> GetCardsByLanguage(string language);

        Task AddCardFileAsync(IFormFile uploadedFile, CardFileDTO cardFile);

        Task<bool> UpdateCardFileAsync(IFormFile uploadedFile, CardFileDTO cardFile);

        Task<bool> UserOwnsCardFileAsync(int cardFileId, string getUserId);
    }
}