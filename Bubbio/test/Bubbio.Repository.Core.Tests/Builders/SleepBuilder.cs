using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Repository.Core.Documents.Events;

namespace Bubbio.Repository.Core.Tests.Builders
{
    public class SleepBuilder
    {
        private readonly Sleep _sleep;

        public SleepBuilder()
        {
            _sleep = new Sleep
            {
                ChildId = Guid.NewGuid(),
                EventType = EventType.Sleep,
                Transition = Transition.Start
            };
        }

        public SleepBuilder WithChildId(Guid id)
        {
            _sleep.ChildId = id;
            return this;
        }

        public SleepBuilder WithTimestamp(DateTimeOffset timestamp)
        {
            _sleep.Timestamp = timestamp;
            return this;
        }

        public SleepBuilder WithTransition(Transition transition)
        {
            _sleep.Transition = transition;
            return this;
        }

        public Sleep Build()
        {
            return _sleep;
        }
    }
}