using CanaApp.Domain.Contract.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CanaApp.Persistance.GenericRepository
{
    public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenricRepository<TEntity>
        where TEntity : class
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

    }
}
