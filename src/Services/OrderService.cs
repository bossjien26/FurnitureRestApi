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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(DbContextEntity contextEntity)
        => _repository = new OrderRepository(contextEntity);

        public OrderService(IOrderRepository genericRepository)
            => _repository = genericRepository;

        public async Task<Order> GetById(int id) => await _repository.Get(x => x.Id == id);

        public async Task Insert(Order order) => await _repository.Insert(order);

        public IEnumerable<Order> GetMany(int index, int size)
        => _repository.GetAll()
            .Skip((index - 1) * size)
            .Take(size)
            .OrderByDescending(x => x.Id);

        public void Update(Order order) => _repository.Update(order);
    }
}