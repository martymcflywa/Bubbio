﻿using System.Collections;
using System.Collections.Generic;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.Repository.MongoDb.Tests.Theories
{
    public class OrphanDocumentsToInsert : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                ChildExamples.AllChildren
            };
            yield return new object[]
            {
                SleepExamples.AllSleeps
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}