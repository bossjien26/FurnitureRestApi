using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IInventorySpecificationService
    {
        Task Insert(InventorySpecification instance);

        Task<InventorySpecification> GetById(int id);

        IEnumerable<InventorySpecification> GetMany(int index, int size);

        Task<bool> CheckInventoryAndInventorySpecificationIsExist(int inventoryId, int specificationContentId);

        IEnumerable<Inventory> GetInventory(int productId, int[] specificationContents);
    }
}