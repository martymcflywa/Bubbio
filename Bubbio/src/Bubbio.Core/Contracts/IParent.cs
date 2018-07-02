using System;

namespace Bubbio.Core.Contracts
{
    public interface IParent
    {
        Guid Id { get; set; }

        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
    }
}