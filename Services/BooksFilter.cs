using System.ComponentModel.DataAnnotations;

namespace FindbookApi.Services
{
    public class BooksFilter
    {
        private int page;
        private int booksPerPage;

        private const int BOOKS_PER_PAGE = 20;

        public BooksFilter()
        {
            page = 1;
            booksPerPage = BOOKS_PER_PAGE;
        }

        public BooksFilter(int page, int booksPerPage)
        {
            this.page = page > 0 ? page : 1;
            this.booksPerPage = booksPerPage < BOOKS_PER_PAGE ? BOOKS_PER_PAGE : booksPerPage;
        }

        public int Skip
        {
            get => (page - 1) * booksPerPage;
        }

        public int Take
        {
            get => booksPerPage;
        }
    }
}
