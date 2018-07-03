using System;
using Bubbio.Core;

namespace Bubbio.Tests.Core.Builders
{
    public sealed class ParentBuilder
    {
        private readonly Parent _parent;

        public ParentBuilder()
        {
            _parent = new Parent
            {
                Name = new Name { First = "Kim", Middle = "Chi", Last = "Ponce" }
            };
        }

        public ParentBuilder WithId(Guid id)
        {
            _parent.Id = id;
            return this;
        }

        public ParentBuilder WithFirstName(string first)
        {
            _parent.Name.First = first;
            return this;
        }

        public ParentBuilder WithMiddleName(string middle)
        {
            _parent.Name.Middle = middle;
            return this;
        }

        public ParentBuilder WithLastName(string last)
        {
            _parent.Name.Last = last;
            return this;
        }

        public Parent Build()
        {
            return _parent;
        }
    }
}