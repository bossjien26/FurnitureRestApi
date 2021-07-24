using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace src.Repositories.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        
        EntityState DbSetState(T entity);
        
        IQueryable<T> GetAll();
        
        Task Insert(T entity);
        
        Task MultipleInsert(T entity);
        
        Task Update(T entity);
        
        Task MultipleUpdate(T entity);
        
        Task Delete(T entity);
        
        Task MultipleDelete(T entity);
        
        void AddTracked(T entity);
        
        void ModifyTracked(T entity);

        void DeleteTracked(T entity);
        
        void AsyncInsert(T entity);

        void AsyncMultipleInsert(T entity);
    }
}