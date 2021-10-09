using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderInventoryService
    {
        Task Insert(OrderInventory OrderInventory);

        IEnumerable<OrderInventory> GetUserOrderInventoryMany(int orderId);
    }
}