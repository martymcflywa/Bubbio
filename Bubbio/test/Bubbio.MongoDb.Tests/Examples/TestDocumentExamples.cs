using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.Tests.Core.Builders;
using Bubbio.Tests.Core.Examples;

namespace Bubbio.MongoDb.Tests.Examples
{
    public class TestDocumentExamples
    {
        public List<Guid> Ids => ParentExamples.Ids;
        public List<Parent> AllDocuments => ParentExamples.AllParents.ToList();
        public Parent OneDocument => ParentExamples.OneParent;

        public Parent UpdatedDocument => UpdateDocument(OneDocument);

        public List<TestProjection> AllProjections { get; }
        public TestProjection OneProjection => AllProjections.First();

        public TestDocumentExamples()
        {
            AllProjections = new List<TestProjection>
            {
                new TestProjection
                {
                    Id = AllDocuments[0].Id,
                    Name = AllDocuments[0].Name,
                    Version = AllDocuments[0].Version
                },
                new TestProjection
                {
                    Id = AllDocuments[1].Id,
                    Name = AllDocuments[1].Name,
                    Version = AllDocuments[1].Version
                }
            };
        }

        private static Parent UpdateDocument(Parent parent) =>
            new ParentBuilder()
                .WithId(parent.Id)
                .WithCreated(parent.Created)
                .WithModified(DateTimeOffset.UtcNow)
                .WithName(new Name
                {
                    First = "Updated",
                    Last = "Parent"
                })
                .Build();
    }
}