using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Domain.Tests.Theories
{
    public class TransitionPreviousCurrent : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // Previous is end, current is start
            yield return new object[]
            {
                SleepExamples.End,
                SleepExamples.Start,
                true
            };
            // Previous is start, current is end
            yield return new object[]
            {
                SleepExamples.Start,
                SleepExamples.End,
                true
            };
            // Previous is start, current is start
            yield return new object[]
            {
                SleepExamples.Start,
                SleepExamples.Start,
                false
            };
            // Previous is end, current is end
            yield return new object[]
            {
                SleepExamples.End,
                SleepExamples.End,
                false
            };
            // Previous is null, current is start
            yield return new object[]
            {
                null,
                SleepExamples.Start,
                true
            };
            // Previous is null, current is end
            yield return new object[]
            {
                null,
                SleepExamples.End,
                false
            };
            // Previous is null, current is null
            yield return new object[]
            {
                null,
                null,
                false
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}