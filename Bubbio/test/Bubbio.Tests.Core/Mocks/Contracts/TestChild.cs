using System;
using Bubbio.Core.Contracts;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Tests.Core.Mocks.Contracts
{
    public class TestChild : IChild
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