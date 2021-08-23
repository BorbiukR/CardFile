//using AutoMapper;
//using Business.Interfaces;
//using Business.Models;
//using Business.Validation;
//using Data.Entities;
//using Data.Interfaces;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Business.Services
//{
//    public class BookService : IBookService
//    {
//        private readonly IUnitOfWork _unit;
//        private readonly IMapper _mapper;

//        public BookService(IUnitOfWork unit, IMapper mapper)
//        {
//            _unit = unit;
//            _mapper = mapper;
//        }

//        public Task AddAsync(BookModel model)
//        {
//            if (string.IsNullOrEmpty(model.Author))
//                throw new LibraryException("Уou cannot add a book. Book Author is null or empty", nameof(model));
//            if (string.IsNullOrEmpty(model.Title))
//                throw new LibraryException("Уou cannot add a book. Book Title is null or empty", nameof(model));

//            var mappedBook = _mapper.Map<Book>(model);

//            if (model.Year == default)
//            {
//                _unit.BookRepository.AddAsync(mappedBook);
//                return _unit.SaveAsync();
//            }

//            if (model.Year == 0 || model.Year > 2000)
//                throw new LibraryException("You cannot update the book. Book Year is null or empty", nameof(model));

//            _unit.BookRepository.AddAsync(mappedBook);
//            return _unit.SaveAsync();
//        }

//        public Task DeleteByIdAsync(int modelId)
//        {
//            if (modelId == 0)
//                return null;

//            _unit.BookRepository.DeleteByIdAsync(modelId);
//            return  _unit.SaveAsync();
//        }

//        public IEnumerable<BookModel> GetAll()
//        {
//            var books = _unit.BookRepository.FindAll().ToList();

//            return _mapper.Map<IEnumerable<BookModel>>(books);
//        }

//        public IEnumerable<BookModel> GetByFilter(FilterSearchModel filterSearch) 
//        {
//            List<Book> booksWithFilter;
//            if (filterSearch.Author == default)
//            {         
//                booksWithFilter = _unit.BookRepository
//                    .FindAllWithDetails()
//                    .Where(x => x.Year == filterSearch.Year)
//                    .ToList();

//                return _mapper.Map<IEnumerable<BookModel>>(booksWithFilter);
//            }

//            booksWithFilter = _unit.BookRepository
//                .FindAllWithDetails()
//                .Where(x => x.Author == filterSearch.Author)
//                .ToList();

//            return _mapper.Map<IEnumerable<BookModel>>(booksWithFilter);
//        }

//        public async Task<BookModel> GetByIdAsync(int id)
//        {
//            var book = await _unit.BookRepository.GetByIdWithDetailsAsync(id);
            
//            if (book == null)
//                return null;

//            return _mapper.Map<BookModel>(book);
//        }

//        public Task UpdateAsync(BookModel model)
//        {
//            if (string.IsNullOrEmpty(model.Author))
//                throw new LibraryException("You cannot update the book. Book Author is null or empty", nameof(model));
//            if (string.IsNullOrEmpty(model.Title))
//                throw new LibraryException("You cannot update the book. Book Title is null or empty", nameof(model));

//            var mappedBook = _mapper.Map<Book>(model);

//            if (model.Year == default)
//            {
//                _unit.BookRepository.Update(mappedBook);
//                return _unit.SaveAsync();
//            }

//            if (model.Year == 0 || model.Year > 2000)
//                throw new LibraryException("You cannot update the book. Book Year is null or empty", nameof(model));
   
//            _unit.BookRepository.Update(mappedBook);
//            return _unit.SaveAsync();
//        }
//    }
//}