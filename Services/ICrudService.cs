using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindbookApi.Services
{
    public interface ICrudService<Entity> where Entity : class
    {
        Task<IEnumerable<Entity>> FindAll();
        Task<IEnumerable<Entity>> FindAll(BaseFilter<Entity> filter);
        Task<Entity> Find(int id);
        Task<Entity> Add(Entity entity);
        Task<Entity> Update(Entity entity);
        Task Remove(int id);
    }
}
