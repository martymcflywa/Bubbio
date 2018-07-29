using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentToInsert : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                ParentExamples.OneParent
            };
            yield return new object[]
            {
                ParentExamples.OneParent,
                ChildExamples.OneChild
            };
            yield return new object[]
            {
                ChildExamples.OneChild,
                SleepExamples.OneSleep
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}