using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Helpers;
using Bubbio.Repository.MongoDb.Attributes;
using Bubbio.Repository.MongoDb.Interfaces;
using Bubbio.Repository.MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bubbio.Repository.MongoDb
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        #region Constructors

        static MongoDbContext()
        {
            // Use Guid 0x04 subtype by default.
            MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;
        }

        public MongoDbContext(MongoUrl url)
        {
            var client = new MongoClient(ResolveSettings(url));
            _database = client.GetDatabase(url.DatabaseName);
        }

        public MongoDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        private static MongoClientSettings ResolveSettings(MongoUrl url) =>
            new MongoClientSettings
            {
                ApplicationName = Assembly.GetCallingAssembly().GetName().Name,
                GuidRepresentation = GuidRepresentation.Standard,
                Server = url.Server
            };

        #endregion

        #region IMongoDbContext

        /// <inheritdoc />
        public IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            // TODO: need to 'enforce' CollectionName attribute in IDocuments
            // Alternatively, implement Pluralize
            var collectionName = GetCollectionName<TDocument, TKey>();

            return _database.GetCollection<TDocument>(
                partitionKey.IsEmpty() ?
                    collectionName : $"{partitionKey}-{collectionName}");
        }

        /// <inheritdoc />
        public async Task DropCollectionAsync<TDocument, TKey>(
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collectionName = GetCollectionName<TDocument, TKey>();

            if (partitionKey.IsEmpty())
                await _database.DropCollectionAsync(collectionName, token);

            await _database.DropCollectionAsync($"{partitionKey}-{collectionName}", token);
        }

        /// <inheritdoc />
        public void SetGuidRepresentation(GuidRepresentation representation)
        {
            MongoDefaults.GuidRepresentation = representation;
        }

        #endregion

        /// <summary>
        /// Get collection name from attribute.
        /// </summary>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        private static string GetCollectionName<TDocument, TKey>()
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (typeof(TDocument).GetTypeInfo()
                .GetCustomAttributes(typeof(CollectionName))
                .FirstOrDefault() as CollectionName)?.Name;
        }
    }
}