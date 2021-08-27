using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface ICategoryService
    {
        Task Insert(Category instance);

        Task<Category> GetById(int Id);
    }
}