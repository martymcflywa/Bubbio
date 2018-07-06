using System;
using Bubbio.Core.Contracts;
using Bubbio.Tests.Core.Mocks.Contracts;

namespace Bubbio.Tests.Core.Builders
{
    public sealed class ParentBuilder
    {
        private readonly TestParent _testParent;

        public ParentBuilder()
        {
            _testParent = new TestParent
            {
                Name = new Name { First = "Kim", Middle = "Chi", Last = "Ponce" }
            };
        }

        public ParentBuilder WithId(Guid id)
        {
            _testParent.Id = id;
            return this;
        }

        public ParentBuilder WithFirstName(string first)
        {
            _testParent.Name.First = first;
            return this;
        }

        public ParentBuilder WithMiddleName(string middle)
        {
            _testParent.Name.Middle = middle;
            return this;
        }

        public ParentBuilder WithLastName(string last)
        {
            _testParent.Name.Last = last;
            return this;
        }

        public TestParent Build()
        {
            return _testParent;
        }
    }
}