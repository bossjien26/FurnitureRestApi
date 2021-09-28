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
    }
}