using System.Collections.Generic;
using FindbookApi.Models;

namespace FindbookApi.Services
{
    public interface IBooksService
    {
        List<Book> All(BooksFilter filter);
        Book Add(Book book);
        Book Edit(Book book);
        void Delete(int id);
        int Count();
    }
}
