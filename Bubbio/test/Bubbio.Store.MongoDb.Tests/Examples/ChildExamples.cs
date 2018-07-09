using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Store.MongoDb.Entities;
using Bubbio.Store.MongoDb.Tests.Builders;

namespace Bubbio.Store.MongoDb.Tests.Examples
{
    public class ChildExamples
    {
        public List<Child> AllChildren { get; }
        public Child OneChild => AllChildren.First();

        public ChildExamples()
        {
            AllChildren = new List<Child>
            {
                new ChildBuilder()
                    .Build(),
                new ChildBuilder()
                    .WithFirstName("Test")
                    .WithMiddleName("Child")
                    .WithLastName("Ponce")
                    .WithDateOfBirth(new DateTimeOffset(2018, 1, 17, 3, 55, 0, TimeSpan.FromHours(8)))
                    .WithGender(Gender.Girl)
                    .WithInitialHeight(420)
                    .WithInitialWeight(3600)
                    .WithInitialHeadCircumference(395)
                    .Build()
            };
        }
    }
}