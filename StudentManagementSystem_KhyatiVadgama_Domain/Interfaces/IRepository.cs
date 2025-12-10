using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementSystem_KhyatiVadgama_Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? filter = null,
                                             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                             int? skip = null, int? take = null,
                                             CancellationToken ct = default);
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    }
}
