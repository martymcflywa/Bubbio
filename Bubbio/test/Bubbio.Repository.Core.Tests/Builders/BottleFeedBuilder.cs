using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Repository.Core.Documents.Events;

namespace Bubbio.Repository.Core.Tests.Builders
{
    public class BottleFeedBuilder
    {
        private readonly BottleFeed _bottleFeed;

        public BottleFeedBuilder()
        {
            _bottleFeed = new BottleFeed
            {
                ChildId = Guid.NewGuid(),
                EventType = EventType.BottleFeed,
                Amount = 60
            };
        }

        public BottleFeedBuilder WithChildId(Guid id)
        {
            _bottleFeed.ChildId = id;
            return this;
        }

        public BottleFeedBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _bottleFeed.Timestamp = timestamp;
            return this;
        }

        public BottleFeedBuilder WithAmount(float amount)
        {
            _bottleFeed.Amount = amount;
            return this;
        }

        public BottleFeed Build()
        {
            return _bottleFeed;
        }
    }
}