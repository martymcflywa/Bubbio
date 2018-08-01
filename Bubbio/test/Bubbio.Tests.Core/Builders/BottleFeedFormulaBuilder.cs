using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;

namespace Bubbio.Tests.Core.Builders
{
    public class BottleFeedFormulaBuilder
    {
        private readonly BottleFeedFormula _bottleFeedFormula;

        public BottleFeedFormulaBuilder()
        {
            _bottleFeedFormula = new BottleFeedFormula
            {
                ChildId = Guid.NewGuid(),
                EventType = EventType.BottleFeedFormula,
                Measurement = new Measurement
                {
                    UnitType = UnitType.Millilitre,
                    Amount = 60
                },
                Scoops = 4,
                Brand = "Acme"
            };
        }

        public BottleFeedFormulaBuilder WithChildId(Guid id)
        {
            _bottleFeedFormula.ChildId = id;
            return this;
        }

        public BottleFeedFormulaBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _bottleFeedFormula.Timestamp = timestamp;
            return this;
        }

        public BottleFeedFormulaBuilder WithMeasurement(Measurement measurement)
        {
            _bottleFeedFormula.Measurement = measurement;
            return this;
        }

        public BottleFeedFormulaBuilder WithScoops(int scoops)
        {
            _bottleFeedFormula.Scoops = scoops;
            return this;
        }

        public BottleFeedFormulaBuilder WithBrand(string brand)
        {
            _bottleFeedFormula.Brand = brand;
            return this;
        }

        public BottleFeedFormula Build()
        {
            return _bottleFeedFormula;
        }
    }
}