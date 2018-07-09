using System;
using Bubbio.Core.Helpers;
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

        public override string ToString()
        {
            return Middle.IsEmpty() ? $"{First} {Last}" : $"{First} {Middle} {Last}";
        }
    }
}