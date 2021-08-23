using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Interfaces;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public Task AddAsync(CardTextFileDTO model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int modelId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CardTextFileDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CardTextFileDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CardTextFileDTO> GetCardFilesByСriteria(Expression<Func<CardTextFileDTO, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CardTextFileDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
