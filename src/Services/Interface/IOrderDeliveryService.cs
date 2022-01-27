using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderDeliveryService
    {
        Task Insert(OrderDelivery orderDelivery);

        Task Update(OrderDelivery orderDelivery);

        Task<OrderDelivery> GetByOrderId(int orderId);

        Task<OrderDelivery> GetById(int id);
    }
}