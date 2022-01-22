using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IProductCategoryService
    {
         Task Insert(ProductCategory instance);

        Task<ProductCategory> GetById(int id);
        
        IQueryable<ProductCategory> GetMany(int index, int size);
    }
}