using System.Collections.Generic;
using System.Linq;
using FindbookApi.Models;
using FindbookApi.AppExceptions;

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
            book.Id = 0;
            if (IsDuplicate(book))
                throw new RequestArgumentException("The book with same author and title already exists", 422);
            db.Books.Add(book);
            db.SaveChanges();
            return book;
        }

        public Book Edit(Book book)
        {
            if (db.Books.Find(book.Id) == null)
                throw new RequestArgumentException("The book doesn't exist", 404);
            db.Books.Update(book);
            db.SaveChanges();
            return book;
        }

        public void Delete(int id)
        {
            Book book = db.Books.Find(id);
            if (book == null)
                throw new RequestArgumentException("The book doesn't exist", 404);
            db.Books.Remove(book);
            db.SaveChanges();
        }

        public int Count()
        {
            return db.Books.Count();
        }

        private bool IsDuplicate(Book book) =>
            db.Books.Any(b => b.Title == book.Title && b.Author == book.Author);
    }
}
