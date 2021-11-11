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

        IQueryable<InventoryToOrderInventory> GetJoinProductAndSpecification(int id);

        IEnumerable<Inventory> GetMany(int index, int size);

        IEnumerable<Inventory> GetShowMany(int index, int size);

        void Update(Inventory instance);

        void Delete(Inventory instance);
    }
}