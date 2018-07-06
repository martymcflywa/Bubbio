using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Store;

namespace Bubbio.Core.Contracts
{
    public interface IChild : IEntity<Guid>
    {
        Guid ParentId { get; set; }

        IName Name { get; set; }
        DateTimeOffset DateOfBirth { get; set; }
        Gender Gender { get; set; }

        long InitialHeight { get; set; }
        long InitialWeight { get; set; }
    }
}