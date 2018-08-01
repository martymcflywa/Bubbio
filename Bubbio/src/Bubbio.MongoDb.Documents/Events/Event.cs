using System;
using Bubbio.Core.Attributes;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Constants;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(
        typeof(Event),
        typeof(BiometricUpdate),
        typeof(BottleFeedFormula),
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