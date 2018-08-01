using System;
using Bubbio.Core.Attributes;
using Bubbio.Core.Contracts.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(
        typeof(BiometricUpdate),
        typeof(BottleFeedBreastMilk),
        typeof(BottleFeedFormula))]
    [CollectionName]
    public class MeasureEvent : Event, IMeasureEvent
    {
        public IMeasurement Measurement { get; set; }
    }
}