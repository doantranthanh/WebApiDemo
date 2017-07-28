using System.Collections.Generic;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.DataServices.DataRepository
{
    public interface IBookRepository
    {
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