using System;
using System.Collections.Generic;
using System.Linq;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.DataServices.DataRepository
{
    public sealed class BookRepository : IBookRepository
    {
        private static Lazy<List<Book>> inmemoryBooks = new Lazy<List<Book>>(() => new List<Book>());

        private static List<Book> InmemoryBooks
        {
            get { return inmemoryBooks.Value; }
        }

        public BookRepository()
        {
            if (!inmemoryBooks.Value.Any())
                RecreateInmemoryBooks();
        }

        public IEnumerable<Book> GetAll()
        {
            
            return InmemoryBooks;
        }

        public Book GetById(int id)
        {
            return InmemoryBooks.Find(x => x.Id == id);
        }

        public Book GetByTitle(string title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));
            return InmemoryBooks.Find(x => x.Title == title);
        }

        public IEnumerable<Book> GetAllAvailableBooks()
        {
            return InmemoryBooks.Where(x => x.IsBorrowed == false);
        }

        public IEnumerable<Book> GetAllBorrowedBooks()
        {
            return InmemoryBooks.Where(x => x.IsBorrowed == true);
        }

        public void AddNewBook(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            InmemoryBooks.Add(item);
        }

        public void Delete(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            InmemoryBooks.Remove(item);
        }

        public void DeleteById(int itemId)
        {
            var itemToDelete = InmemoryBooks.Find(x => x.Id == itemId);
            Delete(itemToDelete);
        }

        private void RecreateInmemoryBooks()
        {
            var initBooks = new List<Book>()
            {
                new Book()
                {
                    Id = 1,
                    Title = "Book1",
                    Author = "Author1",
                    Rate = 5,
                    IsBorrowed = true,
                    ReturnedDate = DateTime.Today
                },
                new Book()
                {
                    Id = 2,
                    Title = "Book2",
                    Author = "Author2",
                    Rate = 4,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 3,
                    Title = "Book3",
                    Author = "Author3",
                    Rate = 3,
                    IsBorrowed = true,
                    ReturnedDate = DateTime.Today.AddDays(1)
                },
                new Book()
                {
                    Id = 4,
                    Title = "Book4",
                    Author = "Author4",
                    Rate = 2,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 5,
                    Title = "Book5",
                    Author = "Author5",
                    Rate = 1,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 6,
                    Title = "Book6",
                    Author = "Author6",
                    Rate = 5,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 7,
                    Title = "Book7",
                    Author = "Author7",
                    Rate = 4,
                    IsBorrowed = true,
                    ReturnedDate = DateTime.Today.AddDays(-2)
                },
                new Book()
                {
                    Id = 8,
                    Title = "Book9",
                    Author = "Author9",
                    Rate = 3,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 9,
                    Title = "Book9",
                    Author = "Author9",
                    Rate = 2,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 10,
                    Title = "Book10",
                    Author = "Author10",
                    Rate = 1,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
                new Book()
                {
                    Id = 11,
                    Title = "Book11",
                    Author = "Author11",
                    Rate = 5,
                    IsBorrowed = false,
                    ReturnedDate = null
                },
            };
            inmemoryBooks = new Lazy<List<Book>>(() => initBooks);
        }
    }
}
