using CardFile.DAL;
using CardFile.DAL.Entities;
using CardFile.DAL.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CardFile.Tests.DALTests
{
    public class CardTextFileRepositoryTests
    {
        [Fact]
        public void CardTextFileRepository_FindAll_ReturnsAllValues()
        {
            CancellationToken cts = new CancellationToken();
            using (var context = new CardFileDbContext(UnitTestHelper.GetXUnitTestDbOptions()))
            {
                var cardFilesRepository = new CardTextFileRepository(context);

                var cardFiles = cardFilesRepository.FindAll(cts);

                Assert.Equal(3, cardFiles.Count());
            }
        }

        [Fact]
        public async Task CardTextFileRepository_GetById_ReturnsSingleValue()
        {
            using (var context = new CardFileDbContext(UnitTestHelper.GetXUnitTestDbOptions()))
            {
                var cardFilesRepository = new CardTextFileRepository(context);

                var cardFiles = await cardFilesRepository.GetByIdAsync(1);

                Assert.Equal(1, cardFiles.Id);
                Assert.Equal("sql", cardFiles.Language);
                Assert.Equal("/Files/SQL.txt", cardFiles.Path);
            }
        }

        [Fact]
        public async Task CardTextFileRepository_AddAsync_AddsValueToDatabase()
        {
            using (var context = new CardFileDbContext(UnitTestHelper.GetXUnitTestDbOptions()))
            {
                var cardFilesRepository = new CardTextFileRepository(context);
                var cardFiles = new CardFileEntitie() { Id = 4 };

                await cardFilesRepository.AddAsync(cardFiles);
                await context.SaveChangesAsync();

                Assert.Equal(4, context.CardFiles.Count());
            }
        }

        [Fact]
        public async Task CardTextFileRepository_DeleteByIdAsync_DeletesEntity()
        {
            using (var context = new CardFileDbContext(UnitTestHelper.GetXUnitTestDbOptions()))
            {
                var cardFilesRepository = new CardTextFileRepository(context);

                await cardFilesRepository.DeleteByIdAsync(1);
                await context.SaveChangesAsync();

                Assert.Equal(2, context.CardFiles.Count());
            }
        }

        [Fact]
        public async Task CardTextFileRepository_Update_UpdatesEntity()
        {
            using (var context = new CardFileDbContext(UnitTestHelper.GetXUnitTestDbOptions()))
            {
                var cardFilesRepository = new CardTextFileRepository(context);

                var cardFiles = new CardFileEntitie() { Id = 1, Language = "ef", Path = "/Files/EF.txt" };

                cardFilesRepository.Update(cardFiles);
                await context.SaveChangesAsync();

                Assert.Equal(1, cardFiles.Id);
                Assert.Equal("ef", cardFiles.Language);
                Assert.Equal("/Files/EF.txt", cardFiles.Path);
            }
        }

        [Fact]
        public void CardTextFileRepository_FindByCondition_ReturnsValuesWithSpecialCondition()
        {
            using (var context = new CardFileDbContext(UnitTestHelper.GetXUnitTestDbOptions()))
            {
                var cardFilesRepository = new CardTextFileRepository(context);

                var cardFiles = cardFilesRepository.FindByCondition(x => x.Language == "sql");

                Assert.Equal(1, cardFiles.Count());
            }
        }
    }
}