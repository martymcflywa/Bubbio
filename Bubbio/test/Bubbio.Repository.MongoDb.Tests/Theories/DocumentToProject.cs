using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentToProject : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ParentExamples.OneParent,
                ParentExamples.OneProjectedParent
            };
            yield return new object[]
            {
                ChildExamples.OneChild,
                ChildExamples.OneProjectedChild
            };
            yield return new object[]
            {
                SleepExamples.OneSleep,
                SleepExamples.OneProjectedSleep
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}