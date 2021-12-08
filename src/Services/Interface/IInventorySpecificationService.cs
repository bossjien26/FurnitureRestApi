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

        bool CheckInventoryAndInventorySpecificationIsExist(int inventoryId, int specificationId);

        IEnumerable<int[]> GetInventory( int productId,int[] specificationContents);
    }
}