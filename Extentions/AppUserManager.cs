// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using FindbookApi.Models;
// using FindbookApi.RequestModels;

// namespace FindbookApi
// {
//     public class AppUserManager<TUser> : UserManager<TUser> where TUser : class
//     {
//         public AppUserManager(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor,
//             IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators,
//             IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer,
//             IdentityErrorDescriber errors, IServiceProvider services,
//             ILogger<UserManager<TUser>> logger) : 
//             base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
//         { }

//         public async Task<DateTime> LockOut(int id, int minutes, string reason)
//         {
//             var cancellationToken = new CancellationToken();
//             User user = await Store.FindByIdAsync(id.ToString(), cancellationToken) as User;
            
//             // if (user == null)
//             //     return NotFound();
//             // if (user.LockoutEnd > DateTime.UtcNow)
//             //     return BadRequest(new { error = "The user already locked out" });
//             // IList<string> roles = await userManager.GetRolesAsync(user);
//             // if (roles.Any(r => r == "admin"))
//             //     return Forbid();
//             // int adminId = (await userManager.FindByEmailAsync(User.Identity.Name)).Id;
//             // user.LockoutEnd = DateTime.UtcNow.Add(TimeSpan.FromMinutes(request.Minutes));
//             // user.LockRecord = UpdateLockRecord(user.LockRecord, new LockRecord(request.Reason, adminId));
//             // db.SaveChanges();
//             // return Ok(new { lockoutEnd = user.LockoutEnd });
//         }
//     }
// }
