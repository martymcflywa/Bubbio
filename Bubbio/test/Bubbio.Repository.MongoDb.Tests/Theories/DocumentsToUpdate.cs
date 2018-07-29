using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentsToUpdate : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ParentExamples.AllParents,
                ParentExamples.AllUpdatedParents
            };
            yield return new object[]
            {
                ChildExamples.AllChildren,
                ChildExamples.AllUpdatedChildren
            };
            yield return new object[]
            {
                SleepExamples.AllSleeps,
                SleepExamples.AllUpdatedSleeps
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}