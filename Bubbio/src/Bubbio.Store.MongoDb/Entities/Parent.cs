using System;
using Bubbio.Store.MongoDb.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Store.MongoDb.Entities
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Parent))]
    public class Parent : IParent
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsActive { get; set; } = true;

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; } = DateTimeOffset.Now;

        public Name Name { get; set; }

        public override string ToString()
        {
            return $"{typeof(Parent)} {Id} {Name.First} {Name.Middle} {Name.Last}";
        }
    }
}