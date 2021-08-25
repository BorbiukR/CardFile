using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;

namespace CardFile.DAL.Repositories
{
    public class CardTextFileRepository : Repository<CardFileEntitie>, ICardTextFileRepository
    {
        public CardTextFileRepository(CardFileDbContext cardFileDbContext) : base(cardFileDbContext) { }
    }
}