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
using FindbookApi.RequestModels;

namespace FindbookApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private UserManager<User> userManager { get; }
        private SignInManager<User> signInManager { get; }
        private RoleManager<Role> roleManager { get; }

        public AccountController(ILogger<AccountController> logger, UserManager<User> userManager, SignInManager<User> signInManager,
            RoleManager<Role> roleManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <response code="200">The user created successfully</response>
        /// <response code="422">The user model is not valid</response>
        [HttpPost("[action]")]
        public async Task<ActionResult> SignUp(UserSignUpModel userView)
        {
            var user = new User(userView);
            IdentityResult result = await userManager.CreateAsync(user, userView.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, userView.Role);
                return Ok();
            }
            return UnprocessableEntity(result);
        }

        /// <summary>
        /// Create a new session, get user info &amp; JWT-token
        /// </summary>
        /// <remarks>
        /// The method works both with email and with username.
        /// You can use email or username
        /// </remarks>
        /// <param name="userView"></param>
        /// <response code="200">Returns user info and JWT-token</response>
        /// <response code="422">Invalid email address or password</response>
        [HttpPost("[action]")]
        public async Task<ActionResult> SignIn(UserSignInModel userView)
        {
            User user;
            if (userView.Email != null)
                user = await userManager.FindByEmailAsync(userView.Email);
            else
                user = await userManager.FindByNameAsync(userView.UserName);
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, userView.Password, false);
                if (result.Succeeded)
                    return Ok(new {
                        id = user.Id,
                        userName = user.UserName,   
                        email = user.Email,
                        token = await GetToken(user),
                        roles = await (userManager.GetRolesAsync(user))
                    });
            }
            return UnprocessableEntity(new { error = "Wrong email or password" });
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="userView"></param>
        /// <response code="200">Ok</response>
        /// <response code="401">The user is not signed in</response>
        /// <response code="422">Invalid password</response>
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> ChangePassword(UserChangePassModel userView)
        {
            User user = await userManager.FindByEmailAsync(User.Identity.Name);
            var result = await userManager.ChangePasswordAsync(user, userView.OldPassword, userView.NewPassword);
            if (result.Succeeded)
                return Ok();
            return UnprocessableEntity(new { error = "Wrong password" });
        }

        [NonAction]
        private async Task<string> GetToken(User user)
        {
            var identity = await GetIdentity(user);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtOptions.ISSUER,
                audience: JwtOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        [NonAction]
        private async Task<ClaimsIdentity> GetIdentity(User user)
        {
            IList<string> roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roles[0])
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
