using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Tests.Builders;

namespace Bubbio.Store.MongoDb.Tests.Examples
{
    public class TestDocumentExamples
    {
        public List<TestDocument> AllDocuments { get; }
        public TestDocument OneDocument => AllDocuments.First();

        public TestDocumentExamples()
        {
            AllDocuments = new List<TestDocument>
            {
                new TestDocumentBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Martin Ponce")
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-1))
                    .WithVersion(1)
                    .Build(),
                new TestDocumentBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Chi Ponce")
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-2))
                    .WithVersion(1)
                    .Build(),
                new TestDocumentBuilder()
                    .WithId(Guid.NewGuid())
                    .WithName("Damon Ponce")
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-3))
                    .WithVersion(1)
                    .Build()
            };
        }
    }
}