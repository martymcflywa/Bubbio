using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Repository.Core.Documents.Events;

namespace Bubbio.Repository.Core.Tests.Builders
{
    public class BreastFeedBuilder
    {
        private readonly BreastFeed _breastFeed;

        public BreastFeedBuilder()
        {
            _breastFeed = new BreastFeed
            {
                ChildId = Guid.NewGuid(),
                EventType = EventType.BreastFeed,
                Transition = Transition.Start,
                Side = Side.Left
            };
        }

        public BreastFeedBuilder WithChildId(Guid id)
        {
            _breastFeed.ChildId = id;
            return this;
        }

        public BreastFeedBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _breastFeed.Timestamp = timestamp;
            return this;
        }

        public BreastFeedBuilder WithTransition(Transition transition)
        {
            _breastFeed.Transition = transition;
            return this;
        }

        public BreastFeedBuilder WithSide(Side side)
        {
            _breastFeed.Side = side;
            return this;
        }

        public BreastFeed Build()
        {
            return _breastFeed;
        }
    }
}