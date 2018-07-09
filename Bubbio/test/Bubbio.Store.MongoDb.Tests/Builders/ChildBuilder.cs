using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Store.MongoDb.Entities;

namespace Bubbio.Store.MongoDb.Tests.Builders
{
    public class ChildBuilder
    {
        private readonly Child _child;

        public ChildBuilder()
        {
            _child = new Child
            {
                ParentId = Guid.NewGuid(),
                Name = new Name { First = "Damon", Last = "Ponce" },
                DateOfBirth = new DateTimeOffset(2017, 10, 17, 10, 2, 0, TimeSpan.FromHours(8)),
                Gender = Gender.Boy,
                InitialHeight = 480,
                InitialWeight = 3270,
                InitialHeadCircumference = 370
            };
        }

        public ChildBuilder WithId(Guid id)
        {
            _child.Id = id;
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

        public ChildBuilder WithDateOfBirth(DateTimeOffset dob)
        {
            _child.DateOfBirth = dob;
            return this;
        }

        public ChildBuilder WithGender(Gender gender)
        {
            _child.Gender = gender;
            return this;
        }

        public ChildBuilder WithInitialHeight(float height)
        {
            _child.InitialHeight = height;
            return this;
        }

        public ChildBuilder WithInitialWeight(float weight)
        {
            _child.InitialWeight = weight;
            return this;
        }

        public ChildBuilder WithInitialHeadCircumference(float hc)
        {
            _child.InitialHeadCircumference = hc;
            return this;
        }

        public Child Build()
        {
            return _child;
        }
    }
}