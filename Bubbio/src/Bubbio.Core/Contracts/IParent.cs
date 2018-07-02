using System;

namespace Bubbio.Core.Contracts
{
    public interface IParent
    {
        Guid Id { get; set; }
        IName Name { get; set; }
    }
}