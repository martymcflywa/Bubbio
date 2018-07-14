using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Tests.Examples;
using Bubbio.Store.MongoDb.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests
{
    public class MongoDbRepositoryTests : MongoDbRepositoryTestsBase<TestDocument, Guid, TestProjection>
    {
        private readonly TestDocumentExamples _testDocumentExamples;

        private TestDocument OneDocument =>
            _testDocumentExamples.OneDocument;

        private List<TestDocument> AllDocuments =>
            _testDocumentExamples.AllDocuments;

        private TestDocument UpdatedDocument =>
            _testDocumentExamples.UpdatedDocument;

        private TestProjection OneProjection =>
            _testDocumentExamples.OneProjection;

        private IEnumerable<TestProjection> AllProjections =>
            _testDocumentExamples.AllProjections;

        private TestDocumentExamples.TestProjectionComparer ProjectionComparer =>
            _testDocumentExamples.ProjectionComparer;

        public MongoDbRepositoryTests()
        {
            _testDocumentExamples = new TestDocumentExamples();
        }

        #region No PartitionKey

        #region Create

        [Fact]
        public void AddOne()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => RepositoryAddsOne(OneDocument))
                .Then(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void AddMany()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => RepositoryAddsMany(AllDocuments))
                .Then(_ => RepositoryHas(AllDocuments.Count))
                .BDDfy();
        }

        #endregion

        #region Read

        [Fact]
        public void ReadOneById()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryRetrievesOneById(OneDocument.Id))
                .Then(_ => RepositoryHas(OneDocument));
        }

        [Fact]
        public void ReadOneByFilter()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryRetrievesOneByFilter(d => d.Name.Equals(OneDocument.Name) &&
                                                               d.Version.Equals(OneDocument.Version)))
                .Then(_ => RepositoryHas(OneDocument))
                .BDDfy();
        }

        [Fact]
        public void ReadManyByFilter()
        {
            this.Given(_ => RepositoryContains(AllDocuments))
                .When(_ => RepositoryRetrievesManyByFilter(d => d.Timestamp > DateTimeOffset.UnixEpoch))
                .Then(_ => RepositoryHas(AllDocuments))
                .BDDfy();
        }

        // TODO: ReadAsCursor()

        [Fact]
        public void ReadAny()
        {
            this.Given(_ => RepositoryContains(AllDocuments))
                .When(_ => RepositoryRetrievesAny(d => d.Version.Equals(AllDocuments.First().Version)))
                .Then(_ => RepositoryFoundAny(true))
                .BDDfy();
        }

        [Fact]
        public void ReadCountByFilter()
        {
            this.Given(_ => RepositoryContains(AllDocuments))
                .When(_ => RepositoryRetrievesCountByFilter(d => d.Version.Equals(AllDocuments.First().Version)))
                .Then(_ => RepositoryCounted(AllDocuments.Count))
                .BDDfy();
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateWithUpdatedDocument()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryIsUpdatedBy(UpdatedDocument))
                .Then(_ => RepositoryHas(UpdatedDocument))
                .BDDfy();
        }

        [Fact]
        public void UpdateWithFieldSelector()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryIsUpdatedBy(OneDocument, field => field.Name, UpdatedDocument.Name))
                .Then(_ => RepositoryHas(UpdatedDocument))
                .BDDfy();
        }

        [Fact]
        public void UpdateWithFilterAndFieldSelector()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryIsUpdatedBy(
                    doc => doc.Id.Equals(UpdatedDocument.Id),
                    field => field.Timestamp,
                    UpdatedDocument.Timestamp))
                .Then(_ => RepositoryHas(UpdatedDocument))
                .BDDfy();
        }

        #endregion

        #region Delete

        [Fact]
        public void DeleteOneByDocument()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryDeletesOne(OneDocument))
                .Then(_ => RepositoryDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByFilter()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryDeletesOne(d => d.Name.Equals(OneDocument.Name)))
                .Then(_ => RepositoryDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByDocuments()
        {
            this.Given(_ => RepositoryContains(AllDocuments))
                .When(_ => RepositoryDeletesMany(AllDocuments))
                .Then(_ => RepositoryDeleted(AllDocuments.Count))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByFilter()
        {
            this.Given(_ => RepositoryContains(AllDocuments))
                .When(_ => RepositoryDeletesMany(d => d.Version.Equals(OneDocument.Version)))
                .Then(_ => RepositoryDeleted(AllDocuments.Count))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        #endregion

        #region Project

        [Fact]
        public void ProjectOne()
        {
            this.Given(_ => RepositoryContains(OneDocument))
                .When(_ => RepositoryProjectsOne(
                    d => d.Id.Equals(OneDocument.Id),
                    d => new TestProjection
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Version = d.Version
                    }))
                .Then(_ => DocumentIsProjected(OneProjection, ProjectionComparer))
                .BDDfy();
        }

        [Fact]
        public void ProjectMany()
        {
            this.Given(_ => RepositoryContains(AllDocuments))
                .When(_ => RepositoryProjectsMany(
                    d => d.Version.Equals(OneDocument.Version),
                    d => new TestProjection
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Version = d.Version
                    }))
                .Then(_ => DocumentsAreProjected(AllProjections, ProjectionComparer))
                .BDDfy();
        }

        #endregion

        #endregion
    }
}