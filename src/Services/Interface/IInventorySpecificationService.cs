using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IInventorySpecificationService
    {
        Task Insert(InventorySpecification instance);

        Task<InventorySpecification> GetById(int id);

        IQueryable<InventorySpecification> GetMany(int index, int size);

        Task<bool> CheckInventoryAndInventorySpecificationIsExist(int inventoryId, int specificationContentId);

        IQueryable<Inventory> GetInventory(int productId, int[] specificationContents);

        IQueryable<string> GetSpecificationContent(int inventoryId);
    }
}