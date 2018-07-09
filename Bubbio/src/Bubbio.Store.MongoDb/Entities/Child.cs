using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Store.MongoDb.Contracts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Store.MongoDb.Entities
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Child))]
    public class Child : IChild
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsActive { get; set; } = true;

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; } = DateTimeOffset.Now;

        public Guid ParentId { get; set; }
        public Name Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset DateOfBirth { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Gender Gender { get; set; }

        public float InitialHeight { get; set; }
        public float InitialWeight { get; set; }
        public float InitialHeadCircumference { get; set; }

        public override string ToString()
        {
            return $"{typeof(Parent)} {Id} {Name}";
        }
    }
}