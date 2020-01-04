using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IRepository<TEntity, TKey>
        where TEntity : class
    {
        TEntity Get(TKey id);
        void Create(TEntity item);
        void Delete(TKey id);
        void Delete(TEntity item);
        void Update(TEntity item);
        IEnumerable<TEntity> GetAll();
    }
}
