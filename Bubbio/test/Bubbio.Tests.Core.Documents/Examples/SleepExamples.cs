using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Documents.Builders;

namespace Bubbio.Tests.Core.Documents.Examples
{
    public static class SleepExamples
    {
        public static IEnumerable<Sleep> AllSleeps { get; }
        public static Sleep OneSleep => AllSleeps.First();

        static SleepExamples()
        {
            AllSleeps = new List<Sleep>
            {
                new SleepBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-7))
                    .WithTransition(Transition.Start)
                    .Build(),
                new SleepBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-6))
                    .WithTransition(Transition.Start)
                    .Build(),
                new SleepBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-7)
                        .AddMinutes(30))
                    .WithTransition(Transition.End)
                    .Build(),
                new SleepBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-6)
                        .AddMinutes(90))
                    .WithTransition(Transition.End)
                    .Build()
            };
        }
    }
}