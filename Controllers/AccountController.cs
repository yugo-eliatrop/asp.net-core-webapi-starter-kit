using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using FindbookApi.Models;
using FindbookApi.ViewModels;

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

        [HttpPost("[action]")]
        public async Task<ActionResult> SignUp(UserSignUpView userView)
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

        [HttpPost("[action]")]
        public async Task<ActionResult> SignIn(UserSignInView userView)
        {
            var user = await userManager.FindByEmailAsync(userView.Email);
            var result = await signInManager.CheckPasswordSignInAsync(user, userView.Password, false);
            if (result.Succeeded)
                return Ok(new {
                    token = await GetToken(user),
                    userName = user.UserName,
                    email = user.Email
                });
            return UnprocessableEntity(result);
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
