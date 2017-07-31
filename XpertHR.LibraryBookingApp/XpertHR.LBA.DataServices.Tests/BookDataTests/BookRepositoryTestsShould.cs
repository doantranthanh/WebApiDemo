using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using XpertHR.LBA.DataServices.DataEntities;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.DataServices.Tests.BookDataTests
{
    [TestFixture]
    public class BookRepositoryTestsShould
    {
        private IFixture fixture;
        private BookRepository bookRepository;
        private HashSet<Book> newAddedItems;
        private HashSet<Book> deletedItems;
        private int originalNumberOfBooks;

        [SetUp]
        public void SetUp()
        {
            fixture = new Fixture();
            bookRepository = new BookRepository();
            originalNumberOfBooks = bookRepository.GetAll().Count();
            newAddedItems = new HashSet<Book>();
            deletedItems = new HashSet<Book>();
        }


        [TearDown]
        public void TearDown()
        {
            foreach (var item in newAddedItems)
            {
                bookRepository.Delete(item);
            }

            foreach (var item in deletedItems)
            {
                bookRepository.AddNewBook(item);
            }
        }

        [Test]
        public void BeAbleToGetAllBookFromRepository()
        {
            // Act
            var results = bookRepository.GetAll();

            // Assert
            results.Should().NotBeNull();
            results.Count().Should().Be(originalNumberOfBooks);
        }


        [TestCase(1, "Book1", "Author1", 5, true)]
        public void BeAbleToGetBookById(int id, string title, string author, int rate, bool isBorrowed)
        {
            // Act
            var result = bookRepository.GetById(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Title.Should().Be(title);
            result.Author.Should().Be(author);
            result.Rate.Should().Be(rate);
            result.IsBorrowed.Should().Be(isBorrowed);
        }

        [TestCase(1, "Book1", "Author1", 5, true)]
        public void BeAbleGetBookByTitle(int id, string title, string author, int rate, bool isBorrowed)
        {
            // Act
            var result = bookRepository.GetByTitle(title);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Title.Should().Be(title);
            result.Author.Should().Be(author);
            result.Rate.Should().Be(rate);
            result.IsBorrowed.Should().Be(isBorrowed);
        }

 
        [Test]
        public void BeAbleGetAllAvailableBooks()
        {
            var results = bookRepository.GetAllAvailableBooks();
            results.Should().NotBeNull();
            results.Count().Should().Be(8);
        }

        [Test]
        public void BeAbleGetAllBorrowedBooks()
        {
            var results = bookRepository.GetAllBorrowedBooks();
            results.Should().NotBeNull();
            results.Count().Should().Be(3);
        }

        [Test]
        public void BeAbleToAddANewBookToSystem()
        {
            //Arrange
            var item = fixture.Create<Book>();

            var oldNumberOfBooks = bookRepository.GetAll().Count();

            // Act
            bookRepository.AddNewBook(item);
            newAddedItems.Add(item);
            // Assert
            var newListBooks = bookRepository.GetAll();

            newListBooks.Count().Should().Be(oldNumberOfBooks + 1);
        }

        [TestCase(1)]
        public void BeAbleToDeleteBookById(int itemId)
        {
            //Arrange    
            var oldNumberOfBooks = bookRepository.GetAll().Count();
            var itemToDelete = bookRepository.GetById(itemId);
            // Act
            bookRepository.DeleteById(itemId);
            deletedItems.Add(itemToDelete);
            // Assert
            var newListBooks = bookRepository.GetAll();

            newListBooks.Count().Should().Be(oldNumberOfBooks - 1);
        }

        [Test]
        public async Task BeAbleToGetAllBookFromRepositoryAsync()
        {
            // Act
            var results = await bookRepository.GetAllAsync();

            // Assert
            results.Should().NotBeNull();
            results.Count().Should().Be(originalNumberOfBooks);
        }


        [TestCase(1, "Book1", "Author1", 5, true)]
        public async Task BeAbleToGetBookByIdAsync(int id, string title, string author, int rate, bool isBorrowed)
        {
            // Act
            var result = await bookRepository.GetByIdAsync(id);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Title.Should().Be(title);
            result.Author.Should().Be(author);
            result.Rate.Should().Be(rate);
            result.IsBorrowed.Should().Be(isBorrowed);
        }

        [TestCase(1, "Book1", "Author1", 5, true)]
        public async Task BeAbleGetBookByTitleAsync(int id, string title, string author, int rate, bool isBorrowed)
        {
            // Act
            var result = await bookRepository.GetByTitleAsync(title);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Title.Should().Be(title);
            result.Author.Should().Be(author);
            result.Rate.Should().Be(rate);
            result.IsBorrowed.Should().Be(isBorrowed);
        }

        [Test]
        public void BeAbleGetBookByNullTitle()
        {
            // Act
            var result = Assert.ThrowsAsync<ArgumentNullException>(() => bookRepository.GetByTitleAsync(null));

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public async Task BeAbleGetAllAvailableBooksAsync()
        {
            var results = await bookRepository.GetAllAvailableBooksAsync();
            results.Should().NotBeNull();
            results.Count().Should().Be(8);
        }

        [Test]
        public async Task BeAbleGetAllBorrowedBooksAsync()
        {
            var results = await bookRepository.GetAllBorrowedBooksAsync();
            results.Should().NotBeNull();
            results.Count().Should().Be(3);
        }


        [Test]
        public async Task BeAbleToAddANewBookToSystemAsync()
        {
            //Arrange
            var item = fixture.Create<Book>();

            var oldNumberOfBooks = await bookRepository.GetAllAsync();
            var oldTotal = oldNumberOfBooks.Count();
            // Act
            var result = await bookRepository.AddNewBookAsync(item);
            newAddedItems.Add(item);

            // Assert
            var newListBooks = await bookRepository.GetAllAsync();
            var newTotal = newListBooks.Count();
            result.Should().Be(true);
            newTotal.Should().Be(oldTotal + 1);
        }

        [TestCase(1)]
        public async Task BeAbleToDeleteBookByIdAsync(int itemId)
        {
            //Arrange    
            var oldNumberOfBooks = await bookRepository.GetAllAsync();
            var oldTotal = oldNumberOfBooks.Count();
            var itemToDelete = await bookRepository.GetByIdAsync(itemId);
            // Act
            var result = await bookRepository.DeleteByIdAsync(itemId);
            deletedItems.Add(itemToDelete);
            // Assert
            result.Should().Be(true);
            var newListBooks = await bookRepository.GetAllAsync();
            newListBooks.Count().Should().Be(oldTotal - 1);
        }
    }
}