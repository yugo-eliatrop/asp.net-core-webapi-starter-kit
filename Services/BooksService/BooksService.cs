using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FindbookApi.Models;
using FindbookApi.AppExceptions;

namespace FindbookApi.Services
{
    public class BookService : IBooksService
    {
        private Context dbContext;

        public BookService(Context context)
        {
            dbContext = context;
        }

        public IEnumerable<Book> FindAll() => dbContext.Books.ToList();

        public IEnumerable<Book> FindAll(BaseFilter<Book> filter) =>
            dbContext.Books
                .Where(filter.Predicate)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToList();

        public Book Find(int id) => dbContext.Books.Find(id);

        public Book Add(Book book)
        {
            if (IsDuplicate(book))
                throw new RequestArgumentException("The book with same author and title already exists", 422);
            dbContext.Books.Add(book);
            dbContext.SaveChanges();
            return book;
        }

        public Book Update(Book book)
        {
            if (IsDuplicate(book))
                throw new RequestArgumentException("The book with same author and title already exists", 422);
            dbContext.Books.Update(book);
            dbContext.SaveChanges();
            return book;
        }

        public void Remove(int id)
        {
            Book book = dbContext.Books.Find(id);
            if (book == null)
                throw new RequestArgumentException("The book doesn't exist", 404);
            dbContext.Books.Remove(book);
            dbContext.SaveChanges();
        }

        public int Count() => dbContext.Books.Count();

        private bool IsDuplicate(Book book) =>
            dbContext.Books.Any(b => b.Title == book.Title && b.Author == book.Author && b.Id != book.Id);
    }
}
