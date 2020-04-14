using System.Collections.Generic;
using FindbookApi.Models;

namespace FindbookApi.RequestModels
{
    public class UserInfoModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }

        public UserInfoModel(User user, IList<string> roles)
        {
            Id = user.Id;
            Email = user.Email;
            UserName = user.UserName;
            Roles = roles;
        }
    }
}
