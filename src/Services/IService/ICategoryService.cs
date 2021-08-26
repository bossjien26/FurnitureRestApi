using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface ICategoryService
    {
        Task Insert(Category category);
    }
}