using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IProductService
    {
        Task Insert(Product instance);

        Task<Product> GetById(int id);

        Task<Product> GetShowProdcutById(int id);

        IEnumerable<Product> GetMany(int index, int size);

        IEnumerable<Product> GetShowProductMany(int index, int size);

        IEnumerable<Product> GetAll();

        bool CheckProductToProductCategoryIsExist(int productId, int categoryId);
    }
}