using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Tests.Core.Mocks.Contracts;

namespace Bubbio.Tests.Core.Builders
{
    public sealed class ChildBuilder
    {
        private readonly TestChild _testChild;

        public ChildBuilder()
        {
            _testChild = new TestChild
            {
                ParentId = Guid.NewGuid(),
                Name = new TestName { First = "Damon", Last = "Ponce" },
                DateOfBirth = new DateTimeOffset(2017, 10, 17, 10, 2, 0, TimeSpan.FromHours(8)),
                Gender = Gender.Boy,
                InitialHeight = 480,
                InitialWeight = 3270
            };
        }

        public ChildBuilder WithId(Guid id)
        {
            _testChild.Id = id;
            return this;
        }

        public ChildBuilder WithParentId(Guid id)
        {
            _testChild.ParentId = id;
            return this;
        }

        public ChildBuilder WithFirstName(string first)
        {
            _testChild.Name.First = first;
            return this;
        }

        public ChildBuilder WithMiddleName(string middle)
        {
            _testChild.Name.Middle = middle;
            return this;
        }

        public ChildBuilder WithLastName(string last)
        {
            _testChild.Name.Last = last;
            return this;
        }

        public ChildBuilder WithDateOfBirth(DateTimeOffset dateOfBirth)
        {
            _testChild.DateOfBirth = dateOfBirth;
            return this;
        }

        public ChildBuilder WithGender(Gender gender)
        {
            _testChild.Gender = gender;
            return this;
        }

        public ChildBuilder WithInitialHeight(long height)
        {
            _testChild.InitialHeight = height;
            return this;
        }

        public ChildBuilder WithInitialWeight(long weight)
        {
            _testChild.InitialWeight = weight;
            return this;
        }

        public TestChild Build()
        {
            return _testChild;
        }
    }
}