using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bubbio.Core.Store
{
    public interface IStore<TEntity, in TKey> : IQueryable<TEntity>
        where TEntity : IEntity<TKey>
    {
        Task InsertAsync(TEntity entity);
        Task InsertAsync(IEnumerable<TEntity> entities);

        Task<IEnumerable<TEntity>> GetAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAsync(TEntity entity);

        Task<long> CountAsync();
        Task DeleteAllAsync();
    }
}