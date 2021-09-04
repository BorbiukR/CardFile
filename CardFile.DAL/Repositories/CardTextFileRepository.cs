using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;

namespace CardFile.DAL.Repositories
{
    public class CardTextFileRepository : Repository<CardFileEntitie>, ICardFileRepository
    {
        public CardTextFileRepository(CardFileDbContext cardFileDbContext) 
            : base(cardFileDbContext) 
        { 

        }
    }
}