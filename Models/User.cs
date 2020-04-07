using Microsoft.AspNetCore.Identity;
using FindbookApi.ViewModels;

namespace FindbookApi.Models
{
    public class User : IdentityUser<int>
    {
        public User() : base()
        { }

        public User(UserView view) : base()
        { 
            Email = view.Email;
            UserName = view.UserName;
        }
    }
}
