using System;
using Bubbio.Core.Contracts;
using Bubbio.MongoDb.Documents.Entities;

namespace Bubbio.Tests.Core.Builders
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

        public ParentBuilder(Parent parent)
        {
            _parent = parent;
        }

        public ParentBuilder WithId(Guid id)
        {
            _parent.Id = id;
            return this;
        }

        public ParentBuilder WithCreated(DateTimeOffset created)
        {
            _parent.Created = created;
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