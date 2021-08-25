using CardFile.BLL.DTO;
using System;
using System.Collections.Generic;

namespace CardFile.BLL.Interfaces
{
    public interface ICardFileService : ICrud<CardFileDTO>
    {
        IEnumerable<CardFileDTO> GetCardsByDateOfCreation(DateTime dateTime);

        IEnumerable<CardFileDTO> GetCardsByLanguage(string language);

        void Download(string path, string fileName);

        void Upload(int id, CardFileDTO model);
    }
}