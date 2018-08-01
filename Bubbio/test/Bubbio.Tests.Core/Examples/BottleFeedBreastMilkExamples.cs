using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Builders;

namespace Bubbio.Tests.Core.Examples
{
    public class BottleFeedBreastMilkExamples
    {
        public static IEnumerable<BottleFeedBreastMilk> AllBottleFeeds { get; }
        public static BottleFeedBreastMilk OneBottleFeed => AllBottleFeeds.First();

        static BottleFeedBreastMilkExamples()
        {
            AllBottleFeeds = new List<BottleFeedBreastMilk>
            {
                new BottleFeedBreastMilkBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.MinValue)
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 40
                    })
                    .Build(),
                new BottleFeedBreastMilkBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.MinValue.AddHours(2))
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 40
                    })
                    .Build(),
                new BottleFeedBreastMilkBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.MinValue)
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 40
                    })
                    .Build(),
                new BottleFeedBreastMilkBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.MinValue.AddHours(2))
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 40
                    })
                    .Build(),
            };
        }
    }
}