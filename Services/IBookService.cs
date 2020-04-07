using FindbookApi.Models;

namespace FindbookApi.Services
{
    public interface IBooksService
    {
        Book Add(Book book);
        void Delete(int id);
    }
}
