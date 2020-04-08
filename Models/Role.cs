using Microsoft.AspNetCore.Identity;

namespace FindbookApi.Models
{
    public class Role : IdentityRole<int>
    {
        public Role() : base()
        { }
        
        public Role(string roleName) : base(roleName)
        { }
    }
}
