using System;
using Bubbio.Core.Contracts.Events;
using Bubbio.Repository.Core.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Repository.Core.Documents.Events
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(BottleFeed))]
    [CollectionName]
    public class BottleFeed : Event, IBottleFeed
    {
        public float Amount { get; set; }
    }
}