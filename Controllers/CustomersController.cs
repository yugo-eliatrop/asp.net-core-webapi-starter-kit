using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using FindbookApi.Models;
using FindbookApi.Services;
using FindbookApi.RequestModels;

namespace FindbookApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("Api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly UserManager<User> userManager;

        public CustomersController(ILogger<AccountController> logger, UserManager<User> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
        }

        /// <summary>
        /// Get list of customers
        /// </summary>
        /// <response code="200">Returns list of customer</response>
        /// <response code="401">The user is not signed in</response>
        /// <response code="403">The user does not have rights</response>
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int usersPerPage = 20)
        {
            BaseFilter filter = new BaseFilter(page, usersPerPage);
            IList<string> roles = new List<string>() { "customer" };
            var list = (await userManager.GetUsersInRoleAsync("customer"))
                .Skip(filter.Skip)
                .Take(filter.Take)
                .OrderBy(u => u.Email)
                .Select(u => new UserInfoModel(u, roles));
            return Ok(new { customers = list });
        }

        /// <summary>
        /// Get customer info
        /// </summary>
        /// <response code="200">Returns customer info</response>
        /// <response code="401">The user is not signed in</response>
        /// <response code="403">The user does not have rights</response>
        /// <response code="404">Customer not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> Show(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Any(r => r == "customer"))
                return NotFound();
            return Ok(new UserInfoModel(user, roles));
        }
    }
}
