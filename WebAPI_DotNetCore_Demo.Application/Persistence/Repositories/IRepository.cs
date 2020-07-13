using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Application.Persistence.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task<TEntity> GetAsync(Guid ID, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
