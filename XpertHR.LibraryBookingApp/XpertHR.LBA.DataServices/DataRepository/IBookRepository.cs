using System.Collections.Generic;
using System.Threading.Tasks;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.DataServices.DataRepository
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<Book> GetByTitleAsync(string title);
        Task<IEnumerable<Book>> GetAllAvailableBooksAsync();
        Task<IEnumerable<Book>> GetAllBorrowedBooksAsync();
        Task<bool> AddNewBookAsync(Book item);
        Task<bool> DeleteAsync(Book item);
        Task<bool> DeleteByIdAsync(int itemId);

        IEnumerable<Book> GetAll();
        Book GetById(int id);
        Book GetByTitle(string title);
        IEnumerable<Book> GetAllAvailableBooks();
        IEnumerable<Book> GetAllBorrowedBooks();
        void AddNewBook(Book item);
        void Delete(Book item);
        void DeleteById(int itemId);
    }
}