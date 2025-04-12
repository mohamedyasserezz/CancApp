using CanaApp.Domain.Contract;
using CanaApp.Domain.Contract.Infrastructure;
using CanaApp.Domain.Specification;
using CanaApp.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace CanaApp.Persistance.GenericRepository
{
    internal class GenericRepository<TEntity, TKey>(ApplicationDbContext _dbContext) : IGenricRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        #region Fields

        #endregion
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false, CancellationToken cancellationToken = default)
        {
            return withTracking ?
                await _dbContext.Set<TEntity>().ToListAsync(cancellationToken) :
                await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> specifications, bool withTracking = false)
        => await ApplySpecification(specifications).ToListAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecification<TEntity, TKey> specifications, bool withTracking = false)
        => await ApplySpecification(specifications).CountAsync();

        public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> specifications)
        => await ApplySpecification(specifications).FirstOrDefaultAsync();

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity, TKey> specifications)
          => SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), specifications);
    }
}
