using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.Persistence.Repositories;
using WebAPI_DotNetCore_Demo.Domain.Entities.Bases;

namespace WebAPI_DotNetCore_Demo.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _entities;
        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _entities.AddRangeAsync(entities, cancellationToken);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public async Task<TEntity> GetAsync(Guid ID, CancellationToken cancellationToken = default)
        {
            return await _entities.SingleOrDefaultAsync(entity => entity.ID == ID, cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _entities.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _entities.Where(predicate).ToListAsync(cancellationToken);
        }
    }
}
