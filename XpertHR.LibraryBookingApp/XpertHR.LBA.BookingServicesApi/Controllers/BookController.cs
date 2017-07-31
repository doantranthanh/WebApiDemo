﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using XpertHR.LBA.BookingServicesApi.Filters;
using XpertHR.LBA.DataServices.DataRepository;

namespace XpertHR.LBA.BookingServicesApi.Controllers
{
    [RoutePrefix("api/books")]
    public class BookController : ApiController
    {
        private readonly IBookRepository bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            if (bookRepository == null)
                throw new ArgumentNullException(nameof(bookRepository));
            this.bookRepository = bookRepository;
        }


        [Route("getall")]
        [HttpGet]
        [ItemNotFoundExceptionFilter]
        public async Task<IHttpActionResult> GetAllBooks()
        {          
            var allBooks = await bookRepository.GetAllAsync();           
            return Ok(allBooks);         
        }


        [Route("getallavailable")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllAvailableBooks()
        {
            var allBooks = await bookRepository.GetAllAvailableBooksAsync();
            if (!allBooks.ToList().Any())
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(allBooks); 
        }

        [Route("getbytitle")]
        [HttpGet]
        [ItemNotFoundExceptionFilter]
        public async Task<IHttpActionResult> GetByTitle(string title)
        {
            var allBooks = await bookRepository.GetByTitleAsync(title);
            return Ok(allBooks);
        }

        [Route("ItemNotFound/{id}")]
        [HttpPost]
        [ItemNotFoundExceptionFilter]
        public IHttpActionResult ItemNotFound(int id)
        {
            var service = new CustomExceptionService();
            service.ThrowItemNotFoundException();
            return Ok();
        }
    }

    public class CustomExceptionService
    {
        public void ThrowItemNotFoundException()
        {
            throw new ItemNotFoundException("This is a custom exception.");
        }
    }
}
