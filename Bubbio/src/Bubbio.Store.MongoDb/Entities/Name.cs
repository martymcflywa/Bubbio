using System;
using Bubbio.Store.MongoDb.Contracts;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Store.MongoDb.Entities
{
    [Serializable]
    [BsonDiscriminator(Required = true)]
    [BsonKnownTypes(typeof(Name))]
    public class Name : IName
    {
        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault]
        public string First { get; set; }

        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault]
        public string Middle { get; set; }

        [BsonDefaultValue("")]
        [BsonIgnoreIfDefault]
        public string Last { get; set; }
    }
}