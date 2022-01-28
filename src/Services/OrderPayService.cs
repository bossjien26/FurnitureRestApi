using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class OrderPayService : IOrderPayService
    {
        private IOrderPayRepository _repository;

        public OrderPayService(DbContextEntity dbContextEntity)
        => _repository = new OrderPayRepository(dbContextEntity);

        public OrderPayService(IOrderPayRepository genericRepository)
        => _repository = genericRepository;

        public async Task Insert(OrderPay orderPay) => await _repository.Insert(orderPay);

        public async Task<OrderPay> GetByOrderId(int orderId)
        => await _repository.Get(x => x.OrderId == orderId);
    }
}