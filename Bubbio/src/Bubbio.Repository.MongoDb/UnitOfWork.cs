using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Repository;

namespace Bubbio.Repository.MongoDb
{
    public class UnitOfWork<TDocument, TKey> : IUnitOfWork<TDocument, TKey>
        where TDocument : IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRepository<TDocument, TKey> _repository;

        public UnitOfWork(IRepository<TDocument, TKey> repository)
        {
            _repository = repository;
        }

        #region Create

        /// <inheritdoc />
        public async Task SaveAsync(TDocument entity, CancellationToken token = default)
        {
            await _repository.InsertAsync(entity, token);
        }

        /// <inheritdoc />
        public async Task SaveAsync(IEnumerable<TDocument> entities, CancellationToken token = default)
        {
            await _repository.InsertManyAsync(entities, token);
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public async Task<TDocument> GetAsync(TKey id, CancellationToken token = default)
        {
            if (!await AnyAsync(p => p.Id.Equals(id), token))
                throw new DocumentNotFoundException<TKey>(typeof(TDocument), id);
            return await _repository.GetAsync(id, token);
        }

        /// <inheritdoc />
        public async Task<TDocument> GetAsync(TDocument entity, CancellationToken token = default)
        {
            if (!await AnyAsync(p => p.Id.Equals(entity.Id), token))
                throw new DocumentNotFoundException<TKey>(entity.GetType(), entity.Id);
            return await _repository.GetAsync(entity, token);
        }

        /// <inheritdoc />
        public async Task<TDocument> GetAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<TKey>(typeof(TDocument));
            return await _repository.GetAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TDocument>> GetManyAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<TKey>(typeof(IEnumerable<TDocument>));
            return await _repository.GetManyAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TDocument>> GetManyAsync(Expression<Func<TDocument, bool>> predicate,
            int skip = 0, int take = 50, CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<TKey>(typeof(IEnumerable<TDocument>));
            return await _repository.GetManyAsync(predicate, skip, take, token);
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync(Expression<Func<TDocument, bool>> predicate, CancellationToken token = default)
        {
            return await _repository.AnyAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(CancellationToken token = default)
        {
            return await _repository.CountAsync(token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            return await _repository.CountAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<TProjection> ProjectAsync<TProjection>(Expression<Func<TDocument, bool>> predicate,
                Expression<Func<TDocument, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<TKey>(typeof(TDocument));
            return await _repository.ProjectAsync(predicate, projection, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
                Expression<Func<TDocument, bool>> predicate, Expression<Func<TDocument, TProjection>> projection,
                CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<TKey>(typeof(IEnumerable<TDocument>));
            return await _repository.ProjectManyAsync(predicate, projection, token);
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(TDocument updated, CancellationToken token = default)
        {
            return await _repository.UpdateAsync(updated, token);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TField>(TDocument toUpdate, Expression<Func<TDocument, TField>> selector,
            TField value, CancellationToken token = default)
        {
            return await _repository.UpdateAsync(toUpdate, selector, value, token);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TField>(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TField>> selector, TField value, CancellationToken token = default)
        {
            if (await CountAsync(predicate, token) > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Update, typeof(TDocument));
            return await _repository.UpdateAsync(predicate, selector, value, token);
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public async Task<long> DeleteAsync(TKey id, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteAsync(id, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<TKey>(typeof(TDocument), id);

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteAsync(TDocument entity, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteAsync(entity, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<TKey>(typeof(TDocument), entity.Id);

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            var count = await CountAsync(predicate, token);

            if (count == 0)
                throw new DocumentNotFoundException<TKey>(typeof(TDocument));
            if (count > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Delete, typeof(TDocument));

            return await _repository.DeleteAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync(IEnumerable<TDocument> entities, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteManyAsync(entities, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<TKey>(typeof(TDocument));

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            var deleted = await _repository.DeleteManyAsync(predicate, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<TKey>(typeof(TDocument));

            return deleted;
        }

        #endregion
    }
}