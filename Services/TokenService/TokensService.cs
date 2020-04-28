using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using FindbookApi.Models;
using FindbookApi.RequestModels;
using FindbookApi.AppExceptions;

namespace FindbookApi.Services
{
    public class TokensService : ITokensService
    {
        private readonly Context db;
        private readonly UserManager<User> userManager;
        public TokensService(Context context, UserManager<User> userManager)
        {
            db = context;
            this.userManager = userManager;
        }

        public string GetAccessToken(User user, IList<string> roles)
        {
            var identity = GetIdentity(user, roles);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtOptions.ISSUER,
                audience: JwtOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roles[0])
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public string GetRefreshToken(User user)
        {
            string key = GenerateKey();
            db.RefreshTokens.Add(new RefreshToken(
                user,
                key,
                DateTime.UtcNow.Add(TimeSpan.FromMinutes(JwtOptions.LIFETIME + 5))
            ));
            db.SaveChanges();
            return key;
        }

        private string GenerateKey()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<Tokens> RefreshTokens(Tokens tokens)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokens.AccessToken);
            if (token.ValidTo > DateTime.UtcNow)
                throw new RequestArgumentException("Jwt token is valid yet", 400);
            if (!RefreshTokenIsValid(tokens.RefreshToken))
                throw new RequestArgumentException("Refresh token is out of date", 400);
            string email = token.Claims.Where(c => c.Type == ClaimsIdentity.DefaultNameClaimType).Select(c => c.Value).SingleOrDefault();
            User user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new RequestArgumentException("Jwt token is invalid", 400);
            if (user.LockoutEnd > DateTime.UtcNow)
                throw new RequestArgumentException("The user locked out", 400);
            IList<string> roles = await userManager.GetRolesAsync(user);
            Tokens newTokens = new Tokens() { AccessToken = GetAccessToken(user, roles), RefreshToken = GetRefreshToken(user) };
            return newTokens;
        }

        private bool RefreshTokenIsValid(string key)
        {
            RefreshToken refreshToken = db.RefreshTokens.Where(t => t.Key == key).FirstOrDefault();
            return refreshToken != null && refreshToken.ExpirationTime > DateTime.UtcNow;
        }
    }
}