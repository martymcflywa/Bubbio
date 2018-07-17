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
    [BsonKnownTypes(typeof(Sleep))]
    [CollectionName]
    public class Sleep : Event, ISleep
    {
        [BsonRepresentation(BsonType.String)]
        public Transition Transition { get; set; }
    }
}