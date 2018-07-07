using System;
using Bubbio.Store.MongoDb.Entities;

namespace Bubbio.Store.MongoDb.Tests.Builders
{
    public class ParentBuilder
    {
        private readonly Parent _parent;

        public ParentBuilder()
        {
            _parent = new Parent
            {
                Id = Guid.NewGuid(),
                Name = new Name { First = "Kim", Middle = "Chi", Last = "Phan" }
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