using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FindbookApi.Services;
using FindbookApi.Models;
using FindbookApi.RequestModels;

namespace FindbookApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> logger;
        private IBooksService booksService;

        public BooksController(ILogger<BooksController> logger, IBooksService booksService)
        {
            this.logger = logger;
            this.booksService = booksService;
        }

        /// <summary>
        /// Get list of books
        /// </summary>
        /// <response code="200">Returns list of books</response>
        [HttpGet]
        public ActionResult Index(int page = 1, int booksPerPage = 20)
        {
            BaseFilter<Book> filter = new BaseFilter<Book>(page, booksPerPage);
            return Ok(new { books = booksService.FindAll(filter) });
        }

        /// <summary>
        /// Get book info
        /// </summary>
        /// <response code="200">Returns a book info</response>
        /// <response code="404">Book is not found</response>
        [HttpGet("{id}")]
        public ActionResult Show(int id)
        {
            Book book = booksService.Find(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        /// <summary>
        /// Add new book
        /// </summary>
        /// <response code="200">Returns the newly created book</response>
        /// <response code="401">The user is not signed in</response>
        /// <response code="403">The user does not have rights</response>
        /// <response code="422">The item is not valid</response>
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(BookEditModel model)
        {
            return Ok(booksService.Add(new Book(model)));
        }

        /// <summary>
        /// Update book info
        /// </summary>
        /// <response code="200">Returns the updated book</response>
        /// <response code="401">The user is not signed in</response>
        /// <response code="403">The user does not have rights</response>
        /// <response code="404">Book not found</response>
        /// <response code="422">The item is not valid</response>
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] BookEditModel model)
        {
            Book book = booksService.Find(id);
            if (book == null)
                return NotFound();
            book.Update(model);
            return Ok(booksService.Update(book));
        }

        /// <summary>
        /// Remove book
        /// </summary>
        /// <response code="200">The book deleted successfully</response>
        /// <response code="401">The user is not signed in</response>
        /// <response code="403">The user does not have rights</response>
        /// <response code="404">Book is not found</response>
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public ActionResult Remove(int id)
        {
            booksService.Remove(id);
            return Ok();
        }

        /// <summary>
        /// Get books statistic
        /// </summary>
        /// <response code="200">Returns books statistic</response>
        [HttpGet("[action]")]
        public ActionResult Statistic()
        {
            int count = booksService.Count();
            return Ok(new { count = count });
        }
    }
}
