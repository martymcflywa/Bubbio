using System;
using Bubbio.Core.Contracts;
using Bubbio.Store.MongoDb.Entities;

namespace Bubbio.Store.MongoDb.Tests.Builders
{
    public class ParentEntityBuilder
    {
        private readonly ParentEntity _parentEntity;

        public ParentEntityBuilder()
        {
            _parentEntity = new ParentEntity
            {
                Id = Guid.NewGuid(),
                Name = new Name { First = "Kim", Middle = "Chi", Last = "Phan" }
            };
        }

        public ParentEntityBuilder WithId(Guid id)
        {
            _parentEntity.Id = id;
            return this;
        }

        public ParentEntityBuilder WithFirstName(string first)
        {
            _parentEntity.Name.First = first;
            return this;
        }

        public ParentEntityBuilder WithMiddleName(string middle)
        {
            _parentEntity.Name.Middle = middle;
            return this;
        }

        public ParentEntityBuilder WithLastName(string last)
        {
            _parentEntity.Name.Last = last;
            return this;
        }

        public ParentEntity Build()
        {
            return _parentEntity;
        }
    }
}