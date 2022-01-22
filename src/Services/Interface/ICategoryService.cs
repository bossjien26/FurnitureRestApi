using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Services.Dto;

namespace Services.Interface
{
    public interface ICategoryService
    {
        Task Insert(Category instance);

        Task<Category> GetById(int id);

        IQueryable<Category> GetMany(int index, int size);

        IQueryable<Category> GetChildren(int id);

        List<CategoryRelationChildren> GetCategoryRelationChildren(int index, int size);
    }
}