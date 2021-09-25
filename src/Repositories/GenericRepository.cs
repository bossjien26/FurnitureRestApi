using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DbEntity;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly DbContextEntity _context;

        public readonly DbSet<T> _DbSet;

        public GenericRepository(DbContextEntity context)
        {
            _context = context;
            _DbSet = _context.Set<T>();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        => await _DbSet.FirstOrDefaultAsync(predicate);

        public IQueryable<T> GetAll() => _DbSet.AsQueryable();

        public EntityState DbSetState(T entity) => _context.Entry(entity).State;

        public async Task Insert(T entity)
        {
            _context.Add(entity);
            await SaveChanges();
        }

        public async Task InsertMany(List<T> entity)
        {
            await _DbSet.AddRangeAsync(entity);
            await SaveChanges();
        }

        public void AsyncInsert(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.AddAsync(entity);
        }

        public async Task MultipleInsert(T entity)
        {
            _context.AddRange(entity);
            await SaveChanges();
        }

        public void AsyncMultipleInsert(T entity) => _context.AddRangeAsync(entity);

        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                _context.Update(entity);
                await SaveChanges();
            }
        }

        public async Task MultipleUpdate(T entity)
        {
            _context.UpdateRange(entity);
            await SaveChanges();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                _context.Entry(entity).State = EntityState.Deleted;
                _context.Remove(entity);
                await SaveChanges();
            }
        }

        public async Task MultipleDelete(T entity)
        {
            _context.RemoveRange(entity);
            await SaveChanges();
        }

        public void AddTracked(T entity) => _context.Entry(entity).State = EntityState.Added;

        public void ModifyTracked(T entity) => _context.Entry(entity).State = EntityState.Modified;

        public void DeleteTracked(T entity) => _context.Entry(entity).State = EntityState.Deleted;


        public async Task SaveChanges() => await _context.SaveChangesAsync();
    }
}