using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IProductSpecificationService
    {
        Task Insert(ProductSpecification instance);

        IEnumerable<ProductSpecification> GetMany(int productIds);

        IEnumerable<ProductSpecification> GetOneJoinSpecificationByProductId(int productId, int offset,int[] inventoryId);
    }
}