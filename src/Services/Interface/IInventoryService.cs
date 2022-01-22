using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Services.Dto;

namespace Services.Interface
{
    public interface IInventoryService
    {
        Task Insert(Inventory instance);

        Task<Inventory> GetById(int id);

        Task<Inventory> GetShowById(int id);

        IQueryable<ProductToInventory> GetJoinProduct(int id);

        IQueryable<Inventory> GetMany(int index, int size);

        IQueryable<Inventory> GetShowMany(int index, int size);

        void Update(Inventory instance);

        void Delete(Inventory instance);
    }
}