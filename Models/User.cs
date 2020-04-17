using System.Collections.Generic;
using System;
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

        public string ReasonOfLockOut
        {
            get
            {
                if (IsLockedByAdmin)
                    return LockRecord.Reason;
                return "Account was blocked after several failed login attempts";
            }
        }

        public bool IsLockedByAdmin
        {
            get => LockRecord != null && LockRecord.LockoutEnd.CompareTo(LockoutEnd.Value.DateTime.ToUniversalTime()) == 0;
        }
    }
}
