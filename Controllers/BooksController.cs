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
        private IBooksService booksService;

        public BooksController(ILogger<BooksController> logger, IBooksService booksService)
        {
            this.logger = logger;
            this.booksService = booksService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int booksPerPage = 20)
        {
            BooksFilter filter = new BooksFilter(page, booksPerPage);
            return Ok(new { books = booksService.All(filter) });
        }

        [HttpGet("{id}")]
        public ActionResult Show(int id)
        {
            Book book = booksService.Find(id);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(BookEditModel model)
        {
            Book book = new Book(model);
            return Ok(booksService.Add(book));
        }

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

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            booksService.Delete(id);
            return Ok();
        }

        [HttpGet("[action]")]
        public ActionResult Statistic()
        {
            int count = booksService.Count();
            return Ok(new { count = count });
        }
    }
}
