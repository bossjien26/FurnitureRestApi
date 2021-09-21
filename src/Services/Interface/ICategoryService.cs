using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface ICategoryService
    {
        Task Insert(Category instance);

        Task<Category> GetById(int id);
        
        IEnumerable<Category> GetMany(int index, int size);
    }
}