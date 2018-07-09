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
        public float InitialHeight { get; set; }
        public float InitialWeight { get; set; }
        public float InitialHeadCircumference { get; set; }
    }
}