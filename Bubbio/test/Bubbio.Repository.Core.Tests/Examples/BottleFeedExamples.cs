using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Repository.Core.Documents.Events;
using Bubbio.Repository.Core.Tests.Builders;

namespace Bubbio.Repository.Core.Tests.Examples
{
    public static class BottleFeedExamples
    {
        public static IEnumerable<BottleFeed> AllBottleFeeds { get; }
        public static BottleFeed OneBottleFeed => AllBottleFeeds.First();

        static BottleFeedExamples()
        {
            AllBottleFeeds = new List<BottleFeed>
            {
                new BottleFeedBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-5))
                    .WithAmount(50)
                    .Build(),
                new BottleFeedBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-4))
                    .WithAmount(30)
                    .Build(),
                new BottleFeedBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-5)
                        .AddHours(3))
                    .WithAmount(60)
                    .Build(),
                new BottleFeedBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-4)
                        .AddHours(4))
                    .WithAmount(40)
                    .Build()
            };
        }
    }
}