using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Documents.Builders;

namespace Bubbio.Tests.Core.Documents.Examples
{
    public static class BreastFeedExamples
    {
        public static IEnumerable<BreastFeed> AllBreastFeeds { get; }
        public static BreastFeed OneBreastFeed => AllBreastFeeds.First();

        static BreastFeedExamples()
        {
            AllBreastFeeds = new List<BreastFeed>
            {
                new BreastFeedBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-5))
                    .WithTransition(Transition.Start)
                    .WithSide(Side.Left)
                    .Build(),
                new BreastFeedBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-4))
                    .WithTransition(Transition.Start)
                    .WithSide(Side.Right)
                    .Build(),
                new BreastFeedBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-5)
                        .AddMinutes(30))
                    .WithTransition(Transition.End)
                    .WithSide(Side.Right)
                    .Build(),
                new BreastFeedBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-4)
                        .AddMinutes(25))
                    .WithTransition(Transition.End)
                    .WithSide(Side.Left)
                    .Build()
            };
        }
    }
}