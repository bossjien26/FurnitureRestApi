using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IOrderPayService
    {
        Task Insert(OrderPay orderPay);

        Task<OrderPay> GetByOrderId(int orderId);
    }
}