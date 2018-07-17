using System;
using Bubbio.Core.Contracts;
using Bubbio.Repository.Core.Documents.Entities;

namespace Bubbio.Repository.Core.Tests.Builders
{
    public class ParentBuilder
    {
        private readonly Parent _parent;

        public ParentBuilder()
        {
            _parent = new Parent
            {
                Name = new Name
                {
                    First = "Martin",
                    Middle = "Raymond",
                    Last = "Ponce"
                }
            };
        }

        public ParentBuilder WithId(Guid id)
        {
            _parent.Id = id;
            return this;
        }

        public ParentBuilder WithModified(DateTimeOffset modified)
        {
            _parent.Modified = modified;
            return this;
        }

        public ParentBuilder WithName(Name name)
        {
            _parent.Name = name;
            return this;
        }

        public Parent Build()
        {
            return _parent;
        }
    }
}