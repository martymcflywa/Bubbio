using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.Tests.Core.Builders;

namespace Bubbio.Tests.Core.Examples
{
    public static class ChildExamples
    {
        public static List<Guid> Ids { get; }
        public static IEnumerable<Child> AllChildren { get; }
        public static Child OneChild => AllChildren.First();

        public static IEnumerable<Child> AllUpdatedChildren =>
            AllChildren.Select(c => new ChildBuilder()
                .WithId(c.Id)
                .WithCreated(c.Created.AddMinutes(30))
                .WithModified(c.Modified)
                .WithParentId(c.ParentId)
                .WithName(c.Name)
                .Build());

        public static Child OneUpdatedChild => AllUpdatedChildren.First();

        public static IEnumerable<TestProjection> AllProjectedChildren =>
            AllChildren.Select(c => new TestProjectionBuilder()
                .WithId(c.Id)
                .WithVersion(c.Version)
                .Build());

        public static TestProjection OneProjectedChild => AllProjectedChildren.First();

        static ChildExamples()
        {
            Ids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            AllChildren = new List<Child>
            {
                new ChildBuilder()
                    .WithId(Ids[0])
                    .WithParentId(ParentExamples.Ids[0])
                    .WithName(new Name
                    {
                        First = "Damon",
                        Last = "Ponce"
                    })
                    .WithDateOfBirth(new DateTimeOffset(2017, 10, 17, 10, 2, 0, TimeSpan.FromHours(8)))
                    .WithGender(Gender.Boy)
                    .Build(),
                new ChildBuilder()
                    .WithId(Ids[1])
                    .WithParentId(ParentExamples.Ids[0])
                    .WithName(new Name
                    {
                        First = "Second",
                        Last = "Child"
                    })
                    .WithDateOfBirth(new DateTimeOffset(2018, 1, 1, 1, 0, 0, TimeSpan.FromHours(8)))
                    .WithGender(Gender.Girl)
                    .Build(),
                new ChildBuilder()
                    .WithId(Ids[2])
                    .WithParentId(ParentExamples.Ids[1])
                    .WithName(new Name
                    {
                        First = "Aria",
                        Middle = "Kristine",
                        Last = "Nguyen"
                    })
                    .WithDateOfBirth(new DateTimeOffset(2014, 3, 15, 0, 0, 0, TimeSpan.FromHours(10)))
                    .Build()
            };
        }
    }
}