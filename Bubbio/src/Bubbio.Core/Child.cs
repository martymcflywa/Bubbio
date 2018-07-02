using System;
using Bubbio.Core.Contracts;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core
{
    public class Child : IChild
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ParentId { get; set; }
        public IName Name { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public long InitialHeight { get; set; }
        public long InitialWeight { get; set; }
    }
}