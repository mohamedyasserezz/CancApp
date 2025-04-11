namespace CanaApp.Domain.Contract.Infrastructure
{
    public interface IGenricRepository<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        public Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false, CancellationToken cancellationToken = default);
        public Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> specifications, bool withTracking = false);
        public Task<int> GetCountWithSpecAsync(ISpecification<TEntity, TKey> specifications, bool withTracking = false);

        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> specifications);
        public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        public void Update(TEntity entity);
        public void Delete(TEntity entity);

    }
}
