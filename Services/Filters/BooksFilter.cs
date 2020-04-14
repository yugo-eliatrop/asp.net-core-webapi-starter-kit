namespace FindbookApi.Services
{
    public class BooksFilter : BaseFilter
    {
        public BooksFilter() : base()
        { }

        public BooksFilter(int page, int booksPerPage) : base(page, booksPerPage)
        { }
    }
}
