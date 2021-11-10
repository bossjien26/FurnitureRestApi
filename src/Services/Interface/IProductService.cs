using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IProductService
    {
        Task Insert(Product instance);

        Task<Product> GetById(int id);

        IEnumerable<Product> GetByIdDetail(int id);

        Task<Product> GetShowProdcutById(int id);

        IEnumerable<Product> GetMany(int index, int size);

        IQueryable<Product> GetProductByCategory(int index,int size,int categoryId);

        IEnumerable<Product> GetShowProductMany(int index, int size);

        IEnumerable<Product> GetAll();

        bool CheckProductToProductCategoryIsExist(int productId, int categoryId);
    }
}