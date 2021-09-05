using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface ICartService
    {
        Task Insert(Cart instance);

        Task<Cart> GetById(int id);

        IEnumerable<Cart> GetMany(int index, int size);

        void Update(Cart instance);

        void Delete(Cart instance);
    }
}