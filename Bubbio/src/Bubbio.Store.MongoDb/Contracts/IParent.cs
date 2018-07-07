using System;
using Bubbio.Core.Store;
using Bubbio.Store.MongoDb.Entities;

namespace Bubbio.Store.MongoDb.Contracts
{
    public interface IParent : IEntityCommon<Guid>
    {
        Name Name { get; set; }
    }
}