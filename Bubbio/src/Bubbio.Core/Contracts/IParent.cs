using System;
using Bubbio.Core.Store;

namespace Bubbio.Core.Contracts
{
    public interface IParent : IEntity<Guid>
    {
        IName Name { get; set; }
    }
}