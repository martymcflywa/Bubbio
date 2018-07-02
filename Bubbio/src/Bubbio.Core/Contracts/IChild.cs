using System;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts
{
    public interface IChild
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }

        IName Name { get; set; }
        DateTimeOffset DateOfBirth { get; set; }
        Gender Gender { get; set; }

        long InitialHeight { get; set; }
        long InitialWeight { get; set; }
    }
}