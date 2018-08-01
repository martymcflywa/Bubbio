using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Documents.Events
{
    public class Measurement : IMeasurement
    {
        [BsonRepresentation(BsonType.String)]
        public UnitType UnitType { get; set; }
        public float Amount { get; set; }
    }
}