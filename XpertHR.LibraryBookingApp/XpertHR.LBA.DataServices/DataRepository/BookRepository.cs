using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpertHR.LBA.DataServices.DataEntities;

namespace XpertHR.LBA.DataServices.DataRepository
{
    public sealed class BookRepository : IBookRepository
    {
        private static Lazy<List<Book>> inmemoryBooks = new Lazy<List<Book>>(() => new List<Book>());
        private static object _sync = new object();
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
            lock (_sync)
            {
                return InmemoryBooks;
            }
        }

        public Book GetById(int id)
        {
            lock (_sync)
            {
                return InmemoryBooks.Find(x => x.Id == id);
            }
        }

        public Book GetByTitle(string title)
        {
            lock (_sync)
            {
                if (title == null)
                    throw new ArgumentNullException(nameof(title));
                return InmemoryBooks.Find(x => x.Title == title);
            }
        }

        public IEnumerable<Book> GetAllAvailableBooks()
        {
            lock (_sync)
            {
                return InmemoryBooks.Where(x => x.IsBorrowed == false);
            }
        }

        public IEnumerable<Book> GetAllBorrowedBooks()
        {
            lock (_sync)
            {
                return InmemoryBooks.Where(x => x.IsBorrowed);
            }
        }

        public void AddNewBook(Book item)
        {
            lock (_sync)
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));
                InmemoryBooks.Add(item);
            }
        }

        public void Delete(Book item)
        {
            lock (_sync)
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));
                InmemoryBooks.Remove(item);
            }
        }

        public void DeleteById(int itemId)
        {
            lock (_sync)
            {
                var itemToDelete = InmemoryBooks.Find(x => x.Id == itemId);
                Delete(itemToDelete);
            }
        }

        public Task<List<Book>> GetAllAsync()
        {
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    var allBooks = InmemoryBooks;
                    return allBooks;
                }
            });
        }

        public Task<Book> GetByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    var bookById = InmemoryBooks.ToList().Find(x => x.Id == id);
                    return bookById;
                }
            });
        }

        public Task<Book> GetByTitleAsync(string title)
        {
            if (title == null)
                throw new ArgumentNullException(nameof(title));
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    var book = InmemoryBooks.ToList().Find(x => x.Title == title);
                    return book;
                }
            });
        }

        public Task<IEnumerable<Book>> GetAllAvailableBooksAsync()
        {
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    var allBorrowedBooks = InmemoryBooks.Where(x => x.IsBorrowed == false);
                    return allBorrowedBooks;
                }
            });
        }

        public Task<IEnumerable<Book>> GetAllBorrowedBooksAsync()
        {
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    var allBorrowedBooks = InmemoryBooks.Where(x => x.IsBorrowed);
                    return allBorrowedBooks;
                }
            });
        }

        public Task<bool> AddNewBookAsync(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    bool result;
                    try
                    {
                        AddNewBook(item);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    return result;
                }
            });
        }

        public Task<bool> DeleteAsync(Book item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    bool result;
                    try
                    {
                        Delete(item);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    return result;
                }
            });
        }

        public Task<bool> DeleteByIdAsync(int itemId)
        {
            return Task.Run(() =>
            {
                lock (_sync)
                {
                    bool result;
                    try
                    {
                        DeleteById(itemId);
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                    return result;
                }
            });
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