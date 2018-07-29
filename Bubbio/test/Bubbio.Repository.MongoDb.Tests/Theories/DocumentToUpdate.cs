using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentToUpdate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ParentExamples.OneParent,
                ParentExamples.OneUpdatedParent
            };
            yield return new object[]
            {
                ChildExamples.OneChild,
                ChildExamples.OneUpdatedChild
            };
            yield return new object[]
            {
                SleepExamples.OneSleep,
                SleepExamples.OneUpdatedSleep
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}