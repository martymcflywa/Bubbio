using System;
using Bubbio.Core;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Tests.Core.Builders
{
    public sealed class ChildBuilder
    {
        private readonly Child _child;

        public ChildBuilder()
        {
            _child = new Child
            {
                Name = new Name { First = "Damon", Last = "Ponce" },
                DateOfBirth = new DateTimeOffset(2017, 10, 17, 10, 2, 0, TimeSpan.FromHours(8)),
                Gender = Gender.Boy,
                InitialHeight = 370,
                InitialWeight = 3270
            };
        }

        public ChildBuilder WithId(Guid id)
        {
            _child.Id = id;
            return this;
        }

        public ChildBuilder WithParentId(Guid id)
        {
            _child.ParentId = id;
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

        public ChildBuilder WithInitialHeight(long height)
        {
            _child.InitialHeight = height;
            return this;
        }

        public ChildBuilder WithInitialWeight(long weight)
        {
            _child.InitialWeight = weight;
            return this;
        }

        public Child Build()
        {
            return _child;
        }
    }
}