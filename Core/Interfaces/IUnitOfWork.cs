using Core.Entities;

namespace Core.Interfaces
{
    //inherit Idiposabkle so when finis with transactio dispose the object
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        //below return no of changes in our db
        Task<int> Complete();
    }
}