using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Builders;

namespace Bubbio.Tests.Core.Examples
{
    public static class SleepExamples
    {
        public static IEnumerable<Sleep> AllSleeps { get; }
        public static Sleep OneSleep => AllSleeps.First();

        public static IEnumerable<Sleep> AllUpdatedSleeps =>
            AllSleeps.Select(s => new SleepBuilder()
                .WithId(s.Id)
                .WithCreated(s.Created.AddMinutes(30))
                .WithModified(s.Modified)
                .WithTransition(s.Transition)
                .WithTimestamp(s.Timestamp)
                .Build());

        public static Sleep OneUpdatedSleep => AllUpdatedSleeps.First();

        public static IEnumerable<TestProjection> AllProjectedSleeps =>
            AllSleeps.Select(s => new TestProjectionBuilder()
                .WithId(s.Id)
                .WithVersion(s.Version)
                .Build());

        public static TestProjection OneProjectedSleep => AllProjectedSleeps.First();

        static SleepExamples()
        {
            AllSleeps = new List<Sleep>
            {
                new SleepBuilder()
                    .WithChildId(ChildExamples.OneChild.Id)
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-7))
                    .WithTransition(Transition.Start)
                    .Build(),
                new SleepBuilder()
                    .WithChildId(ChildExamples.OneChild.Id)
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-7)
                        .AddMinutes(30))
                    .WithTransition(Transition.End)
                    .Build(),
                new SleepBuilder()
                    .WithChildId(ChildExamples.AllChildren.Last().Id)
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-6))
                    .WithTransition(Transition.Start)
                    .Build(),
                new SleepBuilder()
                    .WithChildId(ChildExamples.AllChildren.Last().Id)
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-6)
                        .AddMinutes(30))
                    .WithTransition(Transition.End)
                    .Build()
            };
        }
    }
}