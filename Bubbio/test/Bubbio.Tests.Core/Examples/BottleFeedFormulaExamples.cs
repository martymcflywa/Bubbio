using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Builders;

namespace Bubbio.Tests.Core.Examples
{
    public static class BottleFeedFormulaExamples
    {
        public static IEnumerable<BottleFeedFormula> AllBottleFeeds { get; }
        public static BottleFeedFormula OneBottleFeed => AllBottleFeeds.First();

        static BottleFeedFormulaExamples()
        {
            AllBottleFeeds = new List<BottleFeedFormula>
            {
                new BottleFeedFormulaBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-5))
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 50
                    })
                    .WithScoops(4)
                    .WithBrand("Some brand")
                    .Build(),
                new BottleFeedFormulaBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-4))
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 30
                    })
                    .WithScoops(3)
                    .WithBrand("Another brand")
                    .Build(),
                new BottleFeedFormulaBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-5)
                        .AddHours(3))
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 30
                    })
                    .WithScoops(4)
                    .WithBrand("Some brand")
                    .Build(),
                new BottleFeedFormulaBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-4)
                        .AddHours(4))
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Millilitre,
                        Amount = 50
                    })
                    .WithScoops(3)
                    .WithBrand("Another brand")
                    .Build()
            };
        }
    }
}