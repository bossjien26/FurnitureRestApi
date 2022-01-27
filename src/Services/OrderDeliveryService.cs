using System.Threading.Tasks;
using DbEntity;
using Repositories.Interface;
using Entities;
using Repositories;
using Services.Interface;

namespace Services
{
    public class OrderDeliveryService : IOrderDeliveryService
    {
        private IOrderDeliveryRepository _repository;

        public OrderDeliveryService(DbContextEntity dbContextEntity)
        => _repository = new OrderDeliveryRepository(dbContextEntity);

        public OrderDeliveryService(IOrderDeliveryRepository genericRepository)
        => _repository = genericRepository;

        public async Task Insert(OrderDelivery orderDelivery)
        => await _repository.Insert(orderDelivery);

        public async Task Update(OrderDelivery orderDelivery)
        => await _repository.Update(orderDelivery);

        public async Task<OrderDelivery> GetByOrderId(int orderId)
        => await _repository.Get(x => x.OrderId == orderId);
        public async Task<OrderDelivery> GetById(int id)
        => await _repository.Get(x => x.Id == id);
    }
}