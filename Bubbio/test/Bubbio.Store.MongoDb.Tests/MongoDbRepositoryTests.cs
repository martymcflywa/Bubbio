using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Tests.Examples;
using Bubbio.Store.MongoDb.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests
{
    public class MongoDbRepositoryTests : MongoDbRepositoryTestsBase<TestDocument, Guid>
    {
        private readonly TestDocumentExamples _testDocumentExamples;

        private TestDocument OneDocument =>
            _testDocumentExamples.OneDocument;

        private List<TestDocument> AllDocuments =>
            _testDocumentExamples.AllDocuments;

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

        #endregion
    }
}