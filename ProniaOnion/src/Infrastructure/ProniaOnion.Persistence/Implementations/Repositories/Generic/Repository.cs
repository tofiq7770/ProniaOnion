using Microsoft.EntityFrameworkCore;
using ProniaOnion.Application.Abstraction.Repositories.Generic;
using ProniaOnion.Domain.Entities;
using ProniaOnion.Persistence.Contexts;
using System.Linq.Expressions;

namespace ProniaOnion.Persistence.Implementations.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly DbSet<T> _table;
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _table = context.Set<T>();
            _context = context;
        }

        public IQueryable<T> GetAll(bool isTracking = true, bool isDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table;

            query = _addIncludes(query, includes);

            if (isDeleted) query = query.IgnoreQueryFilters();

            return isTracking ? query : query.AsNoTracking();
        }

        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderExpression = null, bool isDescendting = false, int skip = 0, int take = 0, bool isTracking = true, bool IsDeleted = false, params string[] include)
        {
            IQueryable<T> query = _table.AsQueryable();
            if (expression != null) query = query.Where(expression);

            if (orderExpression is not null)
            {
                if (isDescendting) query = query.OrderByDescending(orderExpression);
                else query = query.OrderBy(orderExpression);
            }

            if (skip != 0) query = query.Skip(skip);

            if (take != 0) query = query.Take(take);

            query = _addIncludes(query, include);


            if (IsDeleted) query = query.IgnoreQueryFilters();

            return isTracking ? query : query.AsNoTracking();
        }
        public async Task<T> GetByIdAsync(int id, bool IsTracking = true, bool IgnoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(x => x.Id == id);

            query = _addIncludes(query, includes);

            if (!IsTracking) query = query.AsNoTracking();

            if (IgnoreQuery) query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, bool IsTracking = true, bool IsDeleted = false, params string[] includes)
        {
            IQueryable<T> query = _table.Where(expression).AsQueryable();

            query = _addIncludes(query, includes);

            if (!IsTracking) query = query.AsNoTracking();

            if (IsDeleted) query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
        }
        public async Task<bool> IsExistsAsync(Expression<Func<T, bool>> expression, bool IsDeleted = false)
        {
            return IsDeleted ? await _table.AnyAsync(expression) : await _table.IgnoreQueryFilters().AnyAsync(expression);


        }
        public void Delete(T entity)
        {
            _table.Remove(entity);
        }
        public void Update(T entity)
        {
            _table.Update(entity);
        }

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            
        }
        public void ReverseSoftDelete(T entity)
        {
            entity.IsDeleted = false;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        private IQueryable<T> _addIncludes(IQueryable<T> query, params string[] include)
        {
            if (include is not null)
            {
                for (int i = 0; i < include.Length; i++)
                {
                    query = query.Include(include[i]);
                }
            }
            return query;
        }
    }
}
