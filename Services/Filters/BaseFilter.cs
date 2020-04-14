namespace FindbookApi.Services
{
    public class BaseFilter
    {
        private int page;
        private int itemsPerPage;

        private const int ITEMS_PER_PAGE = 20;

        public BaseFilter()
        {
            page = 1;
            itemsPerPage = ITEMS_PER_PAGE;
        }

        public BaseFilter(int page, int itemsPerPage)
        {
            this.page = page > 0 ? page : 1;
            this.itemsPerPage = itemsPerPage < ITEMS_PER_PAGE ? ITEMS_PER_PAGE : itemsPerPage;
        }

        public int Skip
        {
            get => (page - 1) * itemsPerPage;
        }

        public int Take
        {
            get => itemsPerPage;
        }
    }
}
