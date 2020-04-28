using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindbookApi.Models;
using FindbookApi.AppExceptions;

namespace FindbookApi.Services
{
    public class BookService : IBooksService
    {
        private readonly Context dbContext;

        public BookService(Context context)
        {
            dbContext = context;
        }

        public async Task<IEnumerable<Book>> FindAll() => await Task.Run(() => dbContext.Books.ToList());

        public async Task<IEnumerable<Book>> FindAll(BaseFilter<Book> filter) =>
            await Task.Run(() => dbContext.Books
                .Where(filter.Predicate)
                .Skip(filter.Skip)
                .Take(filter.Take)
                .ToList());

        public async Task<Book> Find(int id) => await dbContext.Books.FindAsync(id);

        public async Task<Book> Add(Book book)
        {
            if (IsDuplicate(book))
                throw new RequestArgumentException("The book with same author and title already exists", 422);
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<Book> Update(Book book)
        {
            if (IsDuplicate(book))
                throw new RequestArgumentException("The book with same author and title already exists", 422);
            dbContext.Books.Update(book);
            await dbContext.SaveChangesAsync();
            return book;
        }

        public async Task Remove(int id)
        {
            Book book = dbContext.Books.Find(id);
            if (book == null)
                throw new RequestArgumentException("The book doesn't exist", 404);
            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();
        }

        public int Count() => dbContext.Books.Count();

        private bool IsDuplicate(Book book) =>
            dbContext.Books.Any(b => b.Title == book.Title && b.Author == book.Author && b.Id != book.Id);
    }
}
