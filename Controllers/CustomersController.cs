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
        private readonly Context db;

        public CustomersController(ILogger<AccountController> logger, UserManager<User> userManager, Context context)
        {
            this.logger = logger;
            this.userManager = userManager;
            db = context;
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

        /// <summary>
        /// Lock out customer
        /// </summary>
        /// <response code="200">Returns lock info</response>
        /// <response code="400">The customer already locked out</response>
        /// <response code="403">The user does not have rights</response>
        /// <response code="404">Customer not found</response>
        [HttpPost("[action]")]
        public async Task<ActionResult> LockOut(LockOutRequest request)
        {
            User user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return NotFound();
            if (user.LockoutEnd > DateTime.UtcNow)
                return BadRequest(new { error = "The user already locked out" });
            IList<string> roles = await userManager.GetRolesAsync(user);
            if (roles.Any(r => r == "admin"))
                return Forbid();
            user.LockoutEnd = DateTime.UtcNow.Add(TimeSpan.FromMinutes(request.Minutes));
            db.SaveChanges();
            return Ok(new { lockoutEnd = user.LockoutEnd });
        }
    }
}
