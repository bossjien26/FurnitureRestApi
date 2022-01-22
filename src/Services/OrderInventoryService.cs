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
    public class OrderInventoryService : IOrderInventoryService
    {
        private IOrderInventoryRepository _repository;

        public OrderInventoryService(DbContextEntity dbContextEntity)
        => _repository = new OrderInventoryRepository(dbContextEntity);

        public OrderInventoryService(IOrderInventoryRepository genericRepository)
        => _repository = genericRepository;

        public async Task Insert(OrderInventory OrderInventory) => await _repository.Insert(OrderInventory);

        public IQueryable<OrderInventory> GetUserOrderInventoryMany( int orderId)
        => _repository.GetAll().Where(x => x.OrderId == orderId);
    }
}