using System.Collections.Generic;
using System.Threading.Tasks;
using FindbookApi.Models;
using FindbookApi.RequestModels;

namespace FindbookApi.Services
{
    public interface ITokensService
    {
        string GetAccessToken(User user, IList<string> roles);
        string GetRefreshToken(User user);
        Task<Tokens> RefreshTokens(Tokens tokens);
    }
}
