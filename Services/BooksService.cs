using System;
using FindbookApi.Models;

namespace FindbookApi.Services
{
    public class BookService : IBooksService
    {
        private Context db;

        public BookService(Context context)
        {
            db = context;
        }

        public Book Add(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
            return book;
        }

        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChangesAsync();
        }
    }
}
