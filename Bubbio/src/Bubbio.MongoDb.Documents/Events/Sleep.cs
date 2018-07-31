using System;
using Bubbio.Core.Attributes;
using Bubbio.Core.Contracts.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Sleep))]
    [CollectionName]
    public class Sleep : TransitionEvent, ISleep
    {
    }
}