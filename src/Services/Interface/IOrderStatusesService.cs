using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderStatusesService
    {
        Task Insert(OrderStatuses orderStatuses);

        IQueryable<OrderStatuses> GetByOrderId(int orderId);
    }
}