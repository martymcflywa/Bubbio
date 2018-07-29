using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bubbio.Core.Repository
{
    public interface IRepository<TEntity, in TKey>
    {
        #region Create

        Task InsertAsync(TEntity entity, CancellationToken token = default);

        Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        #endregion

        #region Read

        Task<TEntity> GetAsync(TKey id, CancellationToken token = default);

        Task<TEntity> GetAsync(TEntity document, CancellationToken token = default);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken token = default);

        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken token = default);

        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, int skip = 0,
            int take = 50, CancellationToken token = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, CancellationToken token = default);

        Task<long> CountAsync(CancellationToken token = default);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken token = default);

        Task<TProjection> ProjectAsync<TProjection>(Expression<Func<TEntity, bool>> predicate,
                Expression<Func<TEntity, TProjection>> projection, CancellationToken token = default)
            where TProjection : class;

        Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
                Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProjection>> projection,
                CancellationToken token = default)
            where TProjection : class;

        #endregion

        #region Update

        Task<bool> UpdateAsync(TEntity updated, CancellationToken token = default);

        Task<bool> UpdateAsync<TField>(TEntity toUpdate, Expression<Func<TEntity, TField>> selector,
            TField value, CancellationToken token = default);

        Task<bool> UpdateAsync<TField>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TField>> selector, TField value, CancellationToken token = default);

        #endregion

        #region Delete

        Task<long> DeleteAsync(TEntity entity, CancellationToken token = default);

        Task<long> DeleteAsync(TKey id, CancellationToken token = default);

        Task<long> DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        Task<long> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken token = default);

        Task DropAsync(CancellationToken token = default);

        #endregion
    }
}