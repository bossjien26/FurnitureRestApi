using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
using Services.Interface;
using Services.Dto;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(DbContextEntity contextEntity)
        {
            _repository = new CategoryRepository(contextEntity);
        }

        public CategoryService(ICategoryRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(Category instance)
            => await _repository.Insert(instance);

        public async Task<Category> GetById(int id)
            => await _repository.Get(x => x.Id == id);

        /// <summary>
        /// 獲得第一層分類
        /// </summary>
        public IEnumerable<Category> GetMany(int index, int size)
        => _repository.GetAll()
                .Where(r => r.ParentId == null)
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public IEnumerable<Category> GetChildren(int id)
        => _repository.GetAll()
            .Where(r => r.ParentId == id)
            .OrderByDescending(x => x.Id);

        public List<CategoryRelationChildren> GetCategoryRelationChildren(int index, int size)
        {
            var categoryRelationChildren = new List<CategoryRelationChildren>();
            GetMany(index, size).ToList().ForEach(r =>
             {
                 categoryRelationChildren.Add(new CategoryRelationChildren()
                 {
                     Id = r.Id,
                     Name = r.Name,
                     ChildrenCategories = GetChildren(r.Id).Select(r => new ChildrenCategory
                     {
                         Id = r.Id,
                         Name = r.Name
                     }).ToList()
                 });
             });

            return categoryRelationChildren;
        }
    }
}