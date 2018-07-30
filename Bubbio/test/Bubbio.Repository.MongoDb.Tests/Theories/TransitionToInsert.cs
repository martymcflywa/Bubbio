using System;
using System.Collections;
using System.Collections.Generic;
using Bubbio.Core.Repository;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class TransitionToInsert : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // Previous is end, current is start
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    SleepExamples.End
                },
                SleepExamples.Start,
                2L
            };
            // Previous is start, current is end
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    SleepExamples.Start
                },
                SleepExamples.End,
                2L
            };
            // Previous is start, current is start
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    SleepExamples.Start
                },
                SleepExamples.Start,
                1L
            };
            // Previous is end, current is end
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild,
                    SleepExamples.End
                },
                SleepExamples.End,
                1L
            };
            // Previous is null, current is start
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild
                },
                SleepExamples.Start,
                1L
            };
            // Previous is null, current is end
            yield return new object[]
            {
                new List<IDocument<Guid>>
                {
                    ChildExamples.OneChild
                },
                SleepExamples.End,
                0L
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}