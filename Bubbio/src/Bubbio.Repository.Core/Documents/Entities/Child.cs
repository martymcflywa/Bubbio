using System;
using Bubbio.Core.Contracts;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Repository.Core.Attributes;
using Bubbio.Repository.Core.Documents.Constants;
using Bubbio.Repository.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Repository.Core.Documents.Entities
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Child))]
    [CollectionName]
    public class Child : IPartitionDocument<Guid>, IChild
    {
        #region IPartitionDocument

        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PartitionKey { get; set; } = Partitions.Children.ToString();
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; } = DateTimeOffset.UtcNow;
        public int Version { get; set; } = Schema.Version;

        #endregion

        #region IChild

        public Guid ParentId { get; set; }
        public IName Name { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset DateOfBirth { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Gender Gender { get; set; }
        public float InitialHeight { get; set; }
        public float InitialWeight { get; set; }
        public float InitialHeadCircumference { get; set; }

        #endregion
    }
}