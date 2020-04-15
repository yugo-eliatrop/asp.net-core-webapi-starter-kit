using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using FindbookApi.RequestModels;

namespace FindbookApi.Models
{
    public class User : IdentityUser<int>
    {
        public List<RefreshToken> RefreshTokens { get; set; }
        #nullable enable
        public LockRecord? LockRecord { get; set; }
        #nullable disable

        public User() : base()
        { }

        public User(UserSignUpModel model) : base()
        { 
            Email = model.Email;
            UserName = model.UserName;
        }
    }
}
