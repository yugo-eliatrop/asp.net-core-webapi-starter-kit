using System;
using System.Collections.Generic;
using System.Linq;
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
        private Context db;
        private IBooksService booksService;

        public BooksController(ILogger<BooksController> logger, Context context, IBooksService booksService)
        {
            this.logger = logger;
            db = context;
            this.booksService = booksService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int booksPerPage = 20)
        {
            BooksFilter filter = new BooksFilter(page, booksPerPage);
            return Ok(new { books = booksService.All(filter) });
        }

        [Authorize(Roles = "admin")]
        [HttpPost("[action]")]
        public ActionResult Create(Book book)
        {
            Book createdBook = booksService.Add(book);
            if (createdBook == null)
                return UnprocessableEntity(new { error = "The book with same author and title already exists" });
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("[action]")]
        public ActionResult Update(Book book)
        {   
            Book editedBook = booksService.Edit(book);
            return Ok(editedBook);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("[action]")]
        public ActionResult Delete(int id)
        {
            booksService.Delete(id);
            return Ok();
        }

        [Authorize]
        [HttpGet("[action]")]
        public ActionResult Statistic()
        {
            int count = db.Books.Count();
            return Ok(new { count = count });
        }
    }
}
