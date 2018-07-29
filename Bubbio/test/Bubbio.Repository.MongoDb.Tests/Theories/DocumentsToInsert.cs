using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentsToInsert : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,
                ParentExamples.AllParents
            };
            yield return new object[]
            {
                ParentExamples.AllParents,
                ChildExamples.AllChildren
            };
            yield return new object[]
            {
                ChildExamples.AllChildren,
                SleepExamples.AllSleeps
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}