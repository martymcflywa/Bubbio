using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentsToDeleteCascade : IEnumerable<object[]>
    {
        private static readonly List<IEnumerable<IDocument<Guid>>> Existing = new List<IEnumerable<IDocument<Guid>>>
        {
            ParentExamples.AllParents,
            ChildExamples.AllChildren,
            SleepExamples.AllSleeps
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                Existing,
                ParentExamples.AllParents,
                Existing.SelectMany(i => i).Count()
            };
            yield return new object[]
            {
                Existing,
                ChildExamples.AllChildren,
                Existing.SelectMany(i => i)
                    .Count(d => !(d is Parent))
            };
            yield return new object[]
            {
                Existing,
                SleepExamples.AllSleeps,
                Existing.SelectMany(i => i)
                    .Count(d => d is Event)
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}