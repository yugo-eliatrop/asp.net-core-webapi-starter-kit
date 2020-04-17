using System.Collections.Generic;

namespace FindbookApi.Services
{
    public interface ICrudService<Entity> where Entity : class
    {
        IEnumerable<Entity> FindAll();
        IEnumerable<Entity> FindAll(BaseFilter<Entity> filter);
        Entity Find(int id);
        Entity Add(Entity entity);
        Entity Update(Entity entity);
        void Remove(int id);
    }
}
