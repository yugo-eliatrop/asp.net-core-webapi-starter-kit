using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FindbookApi.Services;
using FindbookApi.Models;

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
        
        [HttpGet("[action]")]
        public ActionResult Statistic()
        {
            int count = db.Books.Count();
            return Ok(new { count = count });
        }

        [Authorize]
        [HttpPost("[action]")]
        public ActionResult Create(Book book)
        {
            return Ok(booksService.Add(book));
        }
    }
}
