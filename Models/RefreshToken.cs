using System;

namespace FindbookApi.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime ExpirationTime { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public RefreshToken()
        { }

        public RefreshToken(User user, string key, DateTime expirationTime)
        {
            User = user;
            Key = key;
            ExpirationTime = expirationTime;
        }
    }
}
