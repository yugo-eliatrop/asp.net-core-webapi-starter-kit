using System;
using System.Text;
using System.Security.Cryptography;
using FindbookApi.Models;

namespace FindbookApi.Services
{
    public class AuthService : IAuthService
    {
        private Context db;
        public AuthService(Context context)
        {
            db = context;
        }
        public void SignUp(User user)
        {
            user.Salt = ((new Random()).Next() % 100000).ToString();
            user.Password = GetPasswordDigest(user.Password, user.Salt);
            db.Users.Add(user);
            db.SaveChanges();
        }

        private string GetPasswordDigest(string password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password + salt);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            StringBuilder hash = new StringBuilder(); 
            foreach (byte b in byteHash)  
                hash.Append(string.Format("{0:x2}", b));
            return hash.ToString();
        }
    }
}
