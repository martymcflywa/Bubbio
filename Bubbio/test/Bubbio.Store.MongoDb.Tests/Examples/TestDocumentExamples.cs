using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Tests.Builders;

namespace Bubbio.Store.MongoDb.Tests.Examples
{
    public class TestDocumentExamples
    {
        public List<Guid> Ids { get; }

        public List<TestDocument> AllDocuments { get; }
        public TestDocument OneDocument => AllDocuments.First();

        public TestDocument UpdatedDocument => UpdateDocument(OneDocument);

        public TestDocumentExamples()
        {
            Ids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            AllDocuments = new List<TestDocument>
            {
                new TestDocumentBuilder()
                    .WithId(Ids[0])
                    .WithName("Martin Ponce")
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-1))
                    .WithVersion(1)
                    .Build(),
                new TestDocumentBuilder()
                    .WithId(Ids[1])
                    .WithName("Chi Ponce")
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-2))
                    .WithVersion(1)
                    .Build(),
                new TestDocumentBuilder()
                    .WithId(Ids[2])
                    .WithName("Damon Ponce")
                    .WithTimestamp(DateTimeOffset.UtcNow.AddDays(-3))
                    .WithVersion(1)
                    .Build()
            };
        }

        private static TestDocument UpdateDocument(TestDocument document) =>
            new TestDocument
            {
                Id = document.Id,
                Name = "Updated Document",
                Timestamp = DateTimeOffset.UtcNow,
                Version = 2
            };
    }
}