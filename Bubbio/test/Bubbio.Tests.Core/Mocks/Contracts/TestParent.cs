using System;
using Bubbio.Core.Contracts;

namespace Bubbio.Tests.Core.Mocks.Contracts
{
    public class TestParent : IParent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IName Name { get; set; }
    }
}