using System;
using Bubbio.Core.Attributes;
using Bubbio.Core.Contracts.Events;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(BottleFeedFormula))]
    [CollectionName]
    public class BottleFeedFormula : BottleFeedEvent, IBottleFeedFormula
    {
        public int Scoops { get; set; }
        public string Brand { get; set; }
    }
}