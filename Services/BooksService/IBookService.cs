using System.Collections.Generic;
using FindbookApi.Models;

namespace FindbookApi.Services
{
    public interface IBooksService : ICrudService<Book>
    {
        int Count();
    }
}
