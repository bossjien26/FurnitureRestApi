using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface IProductCategoryService
    {
         Task Insert(ProductCategory instance);

        Task<ProductCategory> GetById(int Id);
        
        IEnumerable<ProductCategory> GetMany(int index, int size);
    }
}