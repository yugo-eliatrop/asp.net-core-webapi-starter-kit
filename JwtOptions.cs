using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
 
namespace FindbookApi
{
    public class JwtOptions
    {
        public const string ISSUER = "FindBookAPI";
        public const string AUDIENCE = "FindBookClient";
        public const int LIFETIME = 15;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            string key = Environment.GetEnvironmentVariable("JWT_KEY") ?? "1234567890qwertyuiop";
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}