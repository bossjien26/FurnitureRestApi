using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class OrderStatusesService : IOrderStatusesService
    {
        private readonly IOrderStatusesRepository _repository;

        public OrderStatusesService(DbContextEntity contextEntity)
        => _repository = new OrderStatusesRepository(contextEntity);

        public OrderStatusesService(IOrderStatusesRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(OrderStatuses orderStatuses)
        => await _repository.Insert(orderStatuses);

        public IQueryable<OrderStatuses> GetByOrderId(int orderId)
        => _repository.GetAll().Where(x => x.OrderId == orderId);

        public async Task InsertDefault(int orderId)
        {
            await _repository.InsertMany(new List<OrderStatuses>(){
                new OrderStatuses(){
                    OrderId = orderId,
                    Status = OrderStatusEnum.Created
                },
                new OrderStatuses(){
                     OrderId = orderId,
                Status = OrderStatusEnum.Processing
                }
            });
        }
    }
}