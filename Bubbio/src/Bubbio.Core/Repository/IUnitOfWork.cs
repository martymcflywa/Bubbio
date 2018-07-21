using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bubbio.Core.Repository
{
    /// <summary>
    /// Exposes unit of work crud operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    public interface IUnitOfWork<TEntity, in TKey>
        where TKey : IEquatable<TKey>
    {
        #region Create

        /// <summary>
        /// Async save many entities.
        /// </summary>
        /// <param name="entities">The entities to save.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task SaveAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        #endregion

        #region Read

        /// <summary>
        /// Async get one entities by its primary key.
        /// </summary>
        /// <param name="id">The primary key to find.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id, CancellationToken token = default);

        /// <summary>
        /// Async get one entity by its equivalent entity.
        /// </summary>
        /// <param name="entity">The entity to find.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TEntity entity, CancellationToken token = default);

        /// <summary>
        /// Async get one entity by a linq predicate.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        /// <summary>
        /// Async get many entities by a linq predicate.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken token = default);

        /// <summary>
        /// Async get many paged entities by a linq predicate. Skip determines where the page
        /// starts, take determines size of page. Default skip = 0, take = 50.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="skip">Start of page, default = 0.</param>
        /// <param name="take">Size of page, default = 50.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> predicate, int skip = 0, int take = 50,
            CancellationToken token = default);

        /// <summary>
        /// Async true if any entities match linq predicate.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        /// <summary>
        /// Async count of entities in the collection.
        /// </summary>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> CountAsync(CancellationToken token = default);

        /// <summary>
        /// Async count of entities that match linq predicate.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        /// <summary>
        /// Async project one entity to one TProjection by a linq predicate.
        /// Ensure only one entity is projected.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="projection">The linq projection expression.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TProjection">The projection type.</typeparam>
        /// <returns></returns>
        Task<TProjection> ProjectAsync<TProjection>(Expression<Func<TEntity, bool>> predicate,
                Expression<Func<TEntity, TProjection>> projection, CancellationToken token = default)
            where TProjection : class;

        /// <summary>
        /// Async project many entities to many TProjection by a linq predicate.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="projection">The linq projection expression.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TProjection">The projection type.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(Expression<Func<TEntity, bool>> predicate,
                Expression<Func<TEntity, TProjection>> projection, CancellationToken token = default)
            where TProjection : class;

        #endregion

        #region Update

        /// <summary>
        /// Async update one entity by its equivalent, mutated entity.
        /// </summary>
        /// <param name="updated">The mutated entity.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TEntity updated, CancellationToken token = default);

        /// <summary>
        /// Async update one entity by its equivalent, unmutated entity,
        /// by updating the selected field with a given value.
        /// </summary>
        /// <param name="toUpdate">The original, unmutated entity.</param>
        /// <param name="selector">The linq field selector expression.</param>
        /// <param name="value">The new value of selected field.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TField">The selected field type.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateAsync<TField>(TEntity toUpdate, Expression<Func<TEntity, TField>> selector, TField value,
            CancellationToken token = default);

        /// <summary>
        /// Async update one entity that matches linq predicate, by updating the
        /// selected field with a given value. Ensure that only one entity is
        /// updated.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="selector">The linq field selector expression.</param>
        /// <param name="value">The new value of selected field.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TField"></typeparam>
        /// <returns></returns>
        Task<bool> UpdateAsync<TField>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TField>> selector, TField value, CancellationToken token = default);

        #endregion

        #region Delete

        /// <summary>
        /// Async delete one entity by its primary key.
        /// </summary>
        /// <param name="id">The primary key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteAsync(TKey id, CancellationToken token = default);

        /// <summary>
        /// Async delete one entity by its equivalent entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteAsync(TEntity entity, CancellationToken token = default);

        /// <summary>
        /// Async delete one entity by a linq predicate.
        /// Ensure only one entity is deleted.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        /// <summary>
        /// Async delete many entities by a collection of equivalent entities.
        /// </summary>
        /// <param name="entities">The collection of entities to delete.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken token = default);

        /// <summary>
        /// Async delete many entities which match a linq predicate.
        /// </summary>
        /// <param name="predicate">The linq predicate.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task<long> DeleteManyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);

        #endregion
    }
}