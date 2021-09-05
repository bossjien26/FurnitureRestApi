using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface IProductSpecificationService
    {
        Task Insert(ProductSpecification instance);

        Task<ProductSpecification> GetById(int id);

        IEnumerable<ProductSpecification> GetMany(int index, int size);
    }
}