using System;
using Bubbio.Repository.MongoDb.Attributes;
using Bubbio.Repository.MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Repository.MongoDb.Tests.Examples
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(TestDocument))]
    [CollectionName("TestDocuments")]
    public class TestDocument : IDocument<Guid>
    {
        [BsonId]
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    }
}