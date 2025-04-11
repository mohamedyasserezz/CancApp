namespace CanaApp.Domain.Contract.Infrastructure
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenricRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>()
            where TEntity : class
            where Tkey : IEquatable<Tkey>;
        Task<int> CompleteAsync();
    }
}
