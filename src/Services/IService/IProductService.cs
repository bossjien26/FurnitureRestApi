using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface IProductService
    {
        Task Insert(Product instance);

        Task<Product> GetById(int id);

        IEnumerable<Product> GetMany(int index, int size);
    }
}