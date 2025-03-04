namespace CanaApp.Domain.Contract.Infrastructure
{
    public interface IGenricRepository<TEntity> where TEntity : class
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false, CancellationToken cancellationToken = default);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);

    }
}
