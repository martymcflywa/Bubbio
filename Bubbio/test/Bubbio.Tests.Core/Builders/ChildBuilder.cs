using System;
using Bubbio.Core.Contracts;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Entities;

namespace Bubbio.Tests.Core.Builders
{
    public class ChildBuilder
    {
        private readonly Child _child;

        public ChildBuilder()
        {
            _child = new Child
            {
                ParentId = Guid.NewGuid(),
                Name = new Name
                {
                    First = "Damon",
                    Last = "Ponce"
                },
                DateOfBirth = new DateTimeOffset(2017, 10, 17, 10, 2, 0, TimeSpan.FromHours(8)),
                Gender = Gender.Boy,
                InitialHeight = 48,
                InitialWeight = 3270,
                InitialHeadCircumference = 37
            };
        }

        public ChildBuilder(Child child)
        {
            _child = child;
        }

        public ChildBuilder WithId(Guid id)
        {
            _child.Id = id;
            return this;
        }

        public ChildBuilder WithCreated(DateTimeOffset created)
        {
            _child.Created = created;
            return this;
        }

        public ChildBuilder WithModified(DateTimeOffset modified)
        {
            _child.Modified = modified;
            return this;
        }

        public ChildBuilder WithParentId(Guid parentId)
        {
            _child.ParentId = parentId;
            return this;
        }

        public ChildBuilder WithName(Name name)
        {
            _child.Name = name;
            return this;
        }

        public ChildBuilder WithFirstName(string first)
        {
            _child.Name.First = first;
            return this;
        }

        public ChildBuilder WithMiddleName(string middle)
        {
            _child.Name.Middle = middle;
            return this;
        }

        public ChildBuilder WithLastName(string last)
        {
            _child.Name.Last = last;
            return this;
        }

        public ChildBuilder WithDateOfBirth(DateTimeOffset dateOfBirth)
        {
            _child.DateOfBirth = dateOfBirth;
            return this;
        }

        public ChildBuilder WithGender(Gender gender)
        {
            _child.Gender = gender;
            return this;
        }

        public Child Build()
        {
            return _child;
        }
    }
}