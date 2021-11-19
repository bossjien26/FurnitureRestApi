using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Services.Dto;

namespace Services.Interface
{
    public interface IProductSpecificationService
    {
        Task Insert(ProductSpecification instance);

        IEnumerable<ProductSpecification> GetMany(int productIds);

        IEnumerable<ProductSpecification> GetByNextSpecification(int productId, int? id);

        IEnumerable<ProductSpecificationJoinSpecification> GetManyJoinSpecification(int productId);

        IQueryable<int> GetOneJoinSpecificationByProductId(int productId, List<int> specificationContents);

        IEnumerable<InventoryIdBySpecifications> GetByInventoryIds(List<int> inventoryIds, int productSpecificationId);

        IEnumerable<InventoryIdBySpecifications> GetBySpecificationContent(int productId, List<int> specifications);
    }
}