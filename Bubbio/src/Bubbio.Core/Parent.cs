using System;
using Bubbio.Core.Contracts;

namespace Bubbio.Core
{
    public class Parent : IParent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IName Name { get; set; }
    }
}