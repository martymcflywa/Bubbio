using System;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Tests.Core.Builders
{
    public class TestProjectionBuilder
    {
        private readonly TestProjection _projection;

        public TestProjectionBuilder()
        {
            _projection = new TestProjection();
        }

        public TestProjectionBuilder WithId(Guid id)
        {
            _projection.Id = id;
            return this;
        }

        public TestProjectionBuilder WithVersion(int version)
        {
            _projection.Version = version;
            return this;
        }

        public TestProjection Build()
        {
            return _projection;
        }
    }
}