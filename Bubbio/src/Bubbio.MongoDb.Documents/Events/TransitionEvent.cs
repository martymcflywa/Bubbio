using System;
using Bubbio.Core.Attributes;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(
        typeof(BreastFeed),
        typeof(Sleep))]
    [CollectionName]
    public abstract class TransitionEvent : Event, ITransitionEvent
    {
        [BsonRepresentation(BsonType.String)]
        public Transition Transition { get; set; }
    }
}