using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using Bubbio.Repository.Core.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Repository.Core.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(BreastFeed))]
    [CollectionName]
    public class BreastFeed : Event, IBreastFeed
    {
        [BsonRepresentation(BsonType.String)]
        public Transition Transition { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Side Side { get; set; }
    }
}