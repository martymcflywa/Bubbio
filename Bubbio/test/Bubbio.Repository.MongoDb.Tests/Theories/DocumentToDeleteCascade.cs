using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Repository;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentToDeleteCascade : IEnumerable<object[]>
    {
        private static readonly List<IDocument<Guid>> Existing = new List<IDocument<Guid>>
        {
            ParentExamples.OneParent,
            ChildExamples.OneChild,
            SleepExamples.OneSleep
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                Existing,
                ParentExamples.OneParent,
                Existing.Count
            };
            yield return new object[]
            {
                Existing,
                ChildExamples.OneChild,
                Existing.Take(2).Count()
            };
            yield return new object[]
            {
                Existing,
                SleepExamples.OneSleep,
                Existing.Take(1).Count()
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}