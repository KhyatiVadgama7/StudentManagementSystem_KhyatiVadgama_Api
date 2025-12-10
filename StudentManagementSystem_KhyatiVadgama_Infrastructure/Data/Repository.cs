using Microsoft.EntityFrameworkCore;
using StudentManagementSystem_KhyatiVadgama_Domain.Interfaces;
using StudentManagementSystem_KhyatiVadgama_Infrastructure.Context;
using System.Linq.Expressions;

namespace StudentManagementSystem_KhyatiVadgama_Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _db;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _db.FindAsync<TEntity>(new object[] { id }, ct);
        }

        public async Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, int? skip = null, int? take = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            foreach (var include in includes)
                query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }


        public async Task AddAsync(TEntity entity, CancellationToken ct = default) => await _db.Set<TEntity>().AddAsync(entity, ct);

        public void Update(TEntity entity) => _db.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _db.Set<TEntity>().Remove(entity);

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => _db.Set<TEntity>().AnyAsync(predicate, ct);
    }
}
