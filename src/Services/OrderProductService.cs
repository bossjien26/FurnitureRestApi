using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class OrderProductService : IOrderProductService
    {
        private IOrderProductRepository _repository;

        public OrderProductService(DbContextEntity dbContextEntity)
        => _repository = new OrderProductRepository(dbContextEntity);

        public OrderProductService(IOrderProductRepository genericRepository)
        => _repository = genericRepository;

        public async Task Insert(OrderProduct orderProduct) => await _repository.Insert(orderProduct);

        public IEnumerable<OrderProduct> GetUserOrderProductMany( int orderId)
        => _repository.GetAll().Where(x => x.OrderId == orderId);
    }
}