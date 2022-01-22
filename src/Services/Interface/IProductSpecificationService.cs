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

        IQueryable<ProductSpecification> GetMany(int productIds);

        IQueryable<ProductSpecification> GetByNextSpecification(int productId, int? id);

        IQueryable<ProductSpecificationJoinSpecification> GetManyJoinSpecification(int productId);

        IQueryable<int> GetOneJoinSpecificationByProductId(int productId, List<int> specificationContents);

        IQueryable<InventoryIdBySpecifications> GetByInventoryIds(List<int> inventoryIds, int productSpecificationId);

        IQueryable<InventoryIdBySpecifications> GetBySpecificationContent(int productId, List<int> specifications);
    }
}