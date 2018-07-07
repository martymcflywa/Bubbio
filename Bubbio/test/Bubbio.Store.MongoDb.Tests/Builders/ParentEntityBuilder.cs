using System;
using Bubbio.Store.MongoDb.Entities;

namespace Bubbio.Store.MongoDb.Tests.Builders
{
    public class ParentEntityBuilder
    {
        private readonly Parent _parent;

        public ParentEntityBuilder()
        {
            _parent = new Parent
            {
                Id = Guid.NewGuid(),
                Name = new Name { First = "Kim", Middle = "Chi", Last = "Phan" }
            };
        }

        public ParentEntityBuilder WithId(Guid id)
        {
            _parent.Id = id;
            return this;
        }

        public ParentEntityBuilder WithFirstName(string first)
        {
            _parent.Name.First = first;
            return this;
        }

        public ParentEntityBuilder WithMiddleName(string middle)
        {
            _parent.Name.Middle = middle;
            return this;
        }

        public ParentEntityBuilder WithLastName(string last)
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