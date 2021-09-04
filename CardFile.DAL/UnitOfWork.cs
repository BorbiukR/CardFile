﻿using CardFile.DAL.Interfaces;
using CardFile.DAL.Repositories;
using Data.Interfaces;
using System.Threading.Tasks;

namespace CardFile.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CardFileDbContext _cardFileDbContext;
        private ICardFileRepository _files;

        public UnitOfWork(CardFileDbContext context)
        {
            _cardFileDbContext = context;
        }

        public ICardFileRepository CardFileRepository
        {
            get
            {
                if (_files == null)
                    _files = new CardTextFileRepository(_cardFileDbContext);

                return _files;
            }
        }

        public async Task<int> SaveAsync() => await _cardFileDbContext.SaveChangesAsync();
    }
}