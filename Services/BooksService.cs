using System.Collections.Generic;
using System.Linq;
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

        public List<Book> All(BooksFilter filter)
        {
            return db.Books
                .OrderBy(x => x.Id)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToList();
        }

        public Book Add(Book book)
        {
            if (book.Id > 0)
                book.Id = 0;
            if (IsDuplicate(book))
                return null;
            db.Books.Add(book);
            db.SaveChanges();
            return book;
        }

        public Book Edit(Book book)
        {
            if (db.Books.Find(book.Id) == null)
                throw new System.ArgumentException("ads");
            db.Books.Update(book);
            db.SaveChanges();
            return book;
        }

        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
            }
        }

        private bool IsDuplicate(Book book) =>
            db.Books.Any(b => b.Title == book.Title && b.Author == book.Author);
    }
}
