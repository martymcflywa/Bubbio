using System;

namespace Bubbio.Core.Contracts
{
    public interface IChild
    {
        Guid Id { get; set; }
        Guid ParentId { get; set; }

        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        DateTimeOffset DateOfBirth { get; set; }

        long InitialHeight { get; set; }
        long InitialWeight { get; set; }
    }
}