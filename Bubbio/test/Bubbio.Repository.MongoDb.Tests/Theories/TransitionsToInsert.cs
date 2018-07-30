using System;
using System.Collections;
using System.Collections.Generic;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Builders;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class TransitionsToInsert : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // Previous is end, curent begins with start, collection in order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue)
                        .Build()
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                3L
            };
            // Previous is start, current begins with end, collection in order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue)
                        .Build()
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                3L
            };
            // Previous is start, current begins with start, collection in order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue)
                        .Build()
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                1L
            };
            // Previous is end, current begins with end, collection in order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue)
                        .Build(),
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                1L
            };
            // Previous is null, current begins with start, collection in order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                2L
            };
            // Previous is null, current begins with end, collection in order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                0L
            };
            // Previous is start, current begins with end, collection out of order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue)
                        .Build()
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                1L
            };
            // Previous is end, current begins with start, collection out of order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    new SleepBuilder(SleepExamples.End)
                        .WithTimestamp(DateTimeOffset.MinValue)
                        .Build()
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                1L
            };
            // Previous is null, current begins with start, collection out of order
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                },
                new List<Event>
                {
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(1))
                        .Build(),
                    new SleepBuilder(SleepExamples.Start)
                        .WithTimestamp(DateTimeOffset.MinValue.AddMinutes(2))
                        .Build()
                },
                0L
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}