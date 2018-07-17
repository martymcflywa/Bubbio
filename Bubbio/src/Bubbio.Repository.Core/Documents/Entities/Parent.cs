using System;
using Bubbio.Core.Contracts;
using Bubbio.Repository.Core.Attributes;
using Bubbio.Repository.Core.Documents.Constants;
using Bubbio.Repository.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Repository.Core.Documents.Entities
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Parent))]
    [CollectionName]
    public class Parent : IPartitionDocument<Guid>, IParent
    {
        #region IPartitionDocument

        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PartitionKey { get; set; } = Partitions.Parents.ToString();
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; } = DateTimeOffset.UtcNow;
        public int Version { get; set; } = Schema.Version;

        #endregion

        #region IParent

        public IName Name { get; set; }

        #endregion

    }
}