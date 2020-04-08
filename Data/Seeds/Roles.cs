using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FindbookApi.Models;

namespace FindbookApi.Seeds
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            // string adminEmail = "admin@example.com";
            // string password = "_Aa123456";
            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new Role("admin"));
            if (await roleManager.FindByNameAsync("customer") == null)
                await roleManager.CreateAsync(new Role("customer"));
            // if (await userManager.FindByNameAsync(adminEmail) == null)
            // {
            //     User admin = new User { Email = adminEmail, UserName = adminEmail };
            //     IdentityResult result = await userManager.CreateAsync(admin, password);
            //     if (result.Succeeded)
            //     {
            //         await userManager.AddToRoleAsync(admin, "admin");
            //     }
            // }
        }
    }
}