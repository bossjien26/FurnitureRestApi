using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;

namespace Services.Service
{
    public class CartService : ICartService
    {
        private ICartRepository _repository;

        public CartService(DbContextEntity entity) => _repository = new CartRepository(entity);

        public CartService(ICartRepository repository) => _repository = repository;

        public async Task Insert(Cart instance) => await _repository.Insert(instance);

        public async Task<Cart> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IEnumerable<Cart> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size);
        }

        public void Update(Cart instance) => _repository.Update(instance);

        public void Delete(Cart instance) => _repository.Delete(instance);
    }
}