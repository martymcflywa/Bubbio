using System;
using Bubbio.Repository.Core.Attributes;
using Bubbio.Repository.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.MongoDb.Tests.Examples
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(TestDocument))]
    [CollectionName("TestDocuments")]
    public class TestDocument : IDocument<Guid>
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Created { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Modified { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}