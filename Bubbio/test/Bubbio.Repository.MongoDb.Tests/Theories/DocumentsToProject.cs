using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentsToProject : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ParentExamples.AllParents,
                ParentExamples.AllProjectedParents
            };
            yield return new object[]
            {
                ChildExamples.AllChildren,
                ChildExamples.AllProjectedChildren
            };
            yield return new object[]
            {
                SleepExamples.AllSleeps,
                SleepExamples.AllProjectedSleeps
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}