namespace CanaApp.Domain.Contract.Infrastructure
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenricRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;
        Task<int> CompleteAsync();
    }
}
