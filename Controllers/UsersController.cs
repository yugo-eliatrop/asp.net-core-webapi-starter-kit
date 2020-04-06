using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FindbookApi.Services;
using FindbookApi.Models;

namespace FindbookApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private IAuthService _authService;

        public UsersController(ILogger<UsersController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("[action]")]
        public ActionResult SignUp(User user)
        {
            _authService.SignUp(user);
            return Ok();
        }
    }
}
