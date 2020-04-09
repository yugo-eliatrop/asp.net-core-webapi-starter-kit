using Microsoft.AspNetCore.Identity;
using FindbookApi.RequestModels;

namespace FindbookApi.Models
{
    public class User : IdentityUser<int>
    {
        public User() : base()
        { }

        public User(UserSignUpModel model) : base()
        { 
            Email = model.Email;
            UserName = model.UserName;
        }
    }
}
