using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;

namespace Bubbio.Tests.Core.Builders
{
    public class BottleFeedBreastMilkBuilder
    {
        private readonly BottleFeedBreastMilk _bottleFeedBreastMilk;

        public BottleFeedBreastMilkBuilder()
        {
            _bottleFeedBreastMilk = new BottleFeedBreastMilk
            {
                ChildId = Guid.NewGuid(),
                EventType = EventType.BottleFeedBreastMilk,
                Measurement = new Measurement
                {
                    UnitType = UnitType.Millilitre,
                    Amount = 40
                }
            };
        }

        public BottleFeedBreastMilkBuilder WithChildId(Guid id)
        {
            _bottleFeedBreastMilk.ChildId = id;
            return this;
        }

        public BottleFeedBreastMilkBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _bottleFeedBreastMilk.Timestamp = timestamp;
            return this;
        }

        public BottleFeedBreastMilkBuilder WithMeasurement(Measurement measurement)
        {
            _bottleFeedBreastMilk.Measurement = measurement;
            return this;
        }

        public BottleFeedBreastMilk Build()
        {
            return _bottleFeedBreastMilk;
        }
    }
}