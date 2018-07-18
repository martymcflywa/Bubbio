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
    [BsonKnownTypes(typeof(BiometricUpdate))]
    [CollectionName]
    public class BiometricUpdate : Event, IBiometricUpdate
    {
        [BsonRepresentation(BsonType.String)]
        public BiometricType BiometricType { get; set; }
        public float Measurement { get; set; }
    }
}