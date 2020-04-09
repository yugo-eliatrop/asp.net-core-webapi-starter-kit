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

        [Authorize(Roles = "admin")]
        [HttpPost("[action]")]
        public ActionResult Create(Book book) => Ok(booksService.Add(book));

        [Authorize(Roles = "admin")]
        [HttpPut("[action]")]
        public ActionResult Update(Book book) => Ok(booksService.Edit(book));

        [Authorize(Roles = "admin")]
        [HttpDelete("[action]/{id}")]
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
