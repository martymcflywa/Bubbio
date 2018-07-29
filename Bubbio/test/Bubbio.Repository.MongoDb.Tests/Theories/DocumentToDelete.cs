﻿using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class DocumentToDelete : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ParentExamples.OneParent
            };
            yield return new object[]
            {
                ChildExamples.OneChild
            };
            yield return new object[]
            {
                SleepExamples.OneSleep
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}