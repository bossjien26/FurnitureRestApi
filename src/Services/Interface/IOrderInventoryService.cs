using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderInventoryService
    {
        Task Insert(OrderInventory OrderInventory);

        IQueryable<OrderInventory> GetUserOrderInventoryMany(int orderId);
    }
}