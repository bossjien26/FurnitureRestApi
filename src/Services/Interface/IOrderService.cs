using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderService
    {
        Task<Order> GetById(int id);

        IQueryable<Order> GetMany(int index, int size);

        Task Insert(Order order);

        void Update(Order order);

        Task<Order> GetUserOrder(int orderId,int userId);

        IQueryable<Order> GetUserOrderMany(int userId, int index, int size);
    }
}