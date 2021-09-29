using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderService
    {
        Task<Order> GetById(int id);

        IEnumerable<Order> GetMany(int index, int size);

        Task Insert(Order order);

        void Update(Order order);

        Task<Order> GetUserOrder(int orderId,int userId);

        IEnumerable<Order> GetUserOrderMany(int userId, int index, int size);
    }
}