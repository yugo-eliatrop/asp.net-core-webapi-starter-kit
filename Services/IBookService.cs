using System.Collections.Generic;
using FindbookApi.Models;

namespace FindbookApi.Services
{
    public interface IBooksService
    {
        List<Book> All(BooksFilter filter);
        Book Find(int id);
        Book Add(Book book);
        Book Update(Book book);
        void Delete(int id);
        int Count();
    }
}
