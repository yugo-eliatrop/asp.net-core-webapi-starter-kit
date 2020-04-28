using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FindbookApi.Models;
using FindbookApi.RequestModels;
using FindbookApi.Services;

namespace FindbookApi.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokensService tokensService;

        public AccountController(ILogger<AccountController> logger, UserManager<User> userManager, SignInManager<User> signInManager,
            ITokensService tokensService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokensService = tokensService;
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
        /// <response code="401">The account locked out. Returns reason of lock</response>
        /// <response code="422">Invalid email address or password</response>
        [HttpPost("[action]")]
        public async Task<ActionResult> SignIn(UserSignInModel userView)
        {
            User user = await userManager.Users
                .Include(u => u.LockRecord)
                .Where(u => u.Email == userView.Email || u.UserName == userView.UserName)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, userView.Password, true);
                if (result.Succeeded)
                {
                    logger.LogInformation($"Account/SignIn: User {user.Email} successfully signed in");
                    IList<string> roles = await userManager.GetRolesAsync(user);
                    return Ok(new {
                        id = user.Id,
                        userName = user.UserName,   
                        email = user.Email,
                        accessToken = tokensService.GetAccessToken(user, roles),
                        refreshToken = tokensService.GetRefreshToken(user),
                        roles
                    });
                }
                else if (result.IsLockedOut)
                {
                    logger.LogInformation($"Account/SignIn: User {user.Email} has been blocked");
                    return Unauthorized(new { error = $"Account blocked: {user.ReasonOfLockOut}" });
                }
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

        /// <summary>
        /// Refresh expired access JWT-token
        /// </summary>
        /// <response code="200">Returns new tokens</response>
        /// <response code="400">Some token error</response>
        [HttpPost("[action]")]
        public async Task<ActionResult> RefreshToken(Tokens request)
        {
            var tokens = await tokensService.RefreshTokens(request);
            return Ok(tokens);
        }
    }
}
