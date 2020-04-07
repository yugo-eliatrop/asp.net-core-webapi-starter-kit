using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FindbookApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> logger;
        private Context db;

        public BooksController(ILogger<BooksController> logger, Context context)
        {
            this.logger = logger;
            db = context;
        }
        
        [HttpGet("[action]")]
        public ActionResult Statistic()
        {
            int count = db.Books.Count();
            return Ok(new { count = count });
        }
    }
}
