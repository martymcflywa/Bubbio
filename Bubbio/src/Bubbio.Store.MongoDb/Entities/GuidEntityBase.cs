using System;
using Bubbio.Core.Store;

namespace Bubbio.Store.MongoDb.Entities
{
    public class GuidEntityBase : IEntity<Guid>
    {
        public Guid Id { get; set; }
    }
}