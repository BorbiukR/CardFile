using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;

namespace CardFile.DAL.Repositories
{
    public class CardTextFileRepository : Repository<CardTextFile>, ICardTextFileRepository
    {
        public CardTextFileRepository(CardFileDbContext cardFileDbContext) : base(cardFileDbContext) { }
    }
}