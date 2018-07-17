using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using Bubbio.Repository.Core.Attributes;
using Bubbio.Repository.Core.Documents.Constants;
using Bubbio.Repository.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Repository.Core.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(
        typeof(Event),
        typeof(BiometricUpdate),
        typeof(BottleFeed),
        typeof(BreastFeed),
        typeof(Sleep))]
    [CollectionName]
    public abstract class Event : IPartitionDocument<Guid>, IEvent
    {
        #region IPartitionDocument

        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PartitionKey { get; set; } = Partitions.Events.ToString();
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; } = DateTimeOffset.UtcNow;
        public int Version { get; set; } = Schema.Version;

        #endregion

        #region IEvent

        public Guid ChildId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
        [BsonRepresentation(BsonType.String)]
        public EventType EventType { get; set; }

        #endregion
    }
}