using FindbookApi.Models;

namespace FindbookApi.Services
{
    public interface IAuthService
    {
        void SignUp(User user);
    }
}
