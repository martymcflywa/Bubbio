using System;
using Bubbio.Core.Store;
using MongoDB.Bson.Serialization.Attributes;

namespace Bubbio.Store.MongoDb.Entities
{
    [BsonDiscriminator(RootClass = true)]
    public class GuidEntityBase : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}