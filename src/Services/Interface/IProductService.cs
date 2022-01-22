using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Services.Dto;

namespace Services.Interface
{
    public interface IProductService
    {
        Task Insert(Product instance);

        Task<Product> GetById(int id);

        IQueryable<ProductInventory> GetByIdDetail(int id);

        Task<Product> GetShowProdcutById(int id);

        IQueryable<Product> GetMany(int index, int size);

        IQueryable<Product> GetProductByCategory(int index,int size,int categoryId);

        IQueryable<Product> GetShowProductMany(int index, int size);

        IQueryable<Product> GetAll();

        bool CheckProductToProductCategoryIsExist(int productId, int categoryId);
    }
}