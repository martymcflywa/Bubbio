using System;
using Bubbio.Core.Contracts;

namespace Bubbio.MongoDb.Tests.Examples
{
    public class TestProjection
    {
        public Guid Id { get; set; }
        public IName Name { get; set; }
        public int Version { get; set; }
    }
}