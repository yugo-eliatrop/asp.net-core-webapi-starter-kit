using System;
using System.Linq.Expressions;

namespace FindbookApi.Services
{
    public class BaseFilter<Entity> where Entity : class
    {
        private int page;
        private int itemsPerPage;
        private Expression<Func<Entity, bool>> predicate;
        private const int ITEMS_PER_PAGE = 20;

        public BaseFilter(int page = 1, int itemsPerPage = ITEMS_PER_PAGE)
        {
            this.page = page > 0 ? page : 1;
            this.itemsPerPage = itemsPerPage < ITEMS_PER_PAGE ? ITEMS_PER_PAGE : itemsPerPage;
            this.predicate = x => true;
        }

        public BaseFilter(int page, int itemsPerPage, Expression<Func<Entity, bool>> predicate)
        {
            this.page = page > 0 ? page : 1;
            this.itemsPerPage = itemsPerPage < ITEMS_PER_PAGE ? ITEMS_PER_PAGE : itemsPerPage;
            this.predicate = predicate;
        }

        public int Skip
        {
            get => (page - 1) * itemsPerPage;
        }

        public int Take { get => itemsPerPage; }

        public Expression<Func<Entity, bool>> Predicate { get => predicate; }
    }
}
