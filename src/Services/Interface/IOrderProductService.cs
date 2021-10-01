using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderProductService
    {
        Task Insert(OrderProduct orderProduct);

        IEnumerable<OrderProduct> GetUserOrderProductMany(int orderId);
    }
}