using System;
using System.Linq;
using Bubbio.MongoDb;
using Bubbio.MongoDb.Documents.Constants;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.Repository.MongoDb.Tests.Scenarios;
using Bubbio.Tests.Core;
using Bubbio.Tests.Core.Examples;
using MongoDB.Driver;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Repository.MongoDb.Tests
{
    public class ParentRepositoryTests : RepositoryTestsBase<Parent, Guid, TestProjection>
    {
        private static readonly MongoUrl Url = new MongoUrl(TestConstants.MongoUrl);

        public ParentRepositoryTests()
            : base(new MongoDbRepository(Url), Partitions.Parents.ToString())
        {
        }

        #region Create

        [Fact]
        public void CreateOne()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => RepositoryAddsOne(ParentExamples.OneParent))
                .Then(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void CreateMany()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => RepositoryAddsMany(ParentExamples.AllParents))
                .Then(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        #endregion

        #region Read

        [Fact]
        public void GetOneById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryGetsOneById(ParentExamples.OneParent.Id))
                .Then(_ => DocumentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryGetsOneById(Guid.Empty))
                .Then(_ => DocumentIsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryGetsOneByPredicate(p => p.Name.Equals(ParentExamples.OneParent.Name) &&
                                                             p.Version.Equals(ParentExamples.OneParent.Version)))
                .Then(_ => DocumentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryGetsOneByPredicate(p => p.Version.Equals(0)))
                .Then(_ => DocumentIsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetMany()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsMany(p => p.Created > DateTimeOffset.UnixEpoch))
                .Then(_ => DocumentsAreFound(ParentExamples.AllParents))
                .BDDfy();
        }

        [Fact]
        public void GetManyNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsMany(p => p.Version.Equals(0)))
                .Then(_ => DocumentsAreNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetPaginatedMany()
        {
            const int skip = 0;
            const int take = 1;
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsPaginatedMany(p => p.Version.Equals(1), skip, take))
                .Then(_ => DocumentsAreFound(ParentExamples.AllParents.Take(1)))
                .BDDfy();
        }

        [Fact]
        public void GetPaginatedManyNotFound()
        {
            const int skip = 0;
            const int take = 1;
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsPaginatedMany(p => p.Version.Equals(0), skip, take))
                .Then(_ => DocumentsAreNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetAny()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryFindsAny(p => p.Created > DateTimeOffset.UnixEpoch))
                .Then(_ => RepositoryFoundSomething(true))
                .BDDfy();
        }

        [Fact]
        public void GetAnyNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryFindsAny(p => p.Version.Equals(0)))
                .Then(_ => RepositoryFoundSomething(false))
                .BDDfy();
        }

        [Fact]
        public void GetCount()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsCountForCollection())
                .Then(_ => RepositoryCounted(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void GetCountNone()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => RepositoryGetsCountForCollection())
                .Then(_ => RepositoryCounted(0))
                .BDDfy();
        }

        [Fact]
        public void GetCountByFilter()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsCountByFilter(p => p.Version.Equals(1)))
                .Then(_ => RepositoryCounted(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void GetCountByFilterNone()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryGetsCountByFilter(p => p.Version.Equals(0)))
                .Then(_ => RepositoryCounted(0))
                .BDDfy();
        }

        [Fact]
        public void ProjectOne()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryProjectsOne(
                    p => p.Id.Equals(ParentExamples.OneParent.Id),
                    p => new TestProjection
                    {
                        Id = p.Id,
                        Version = p.Version
                    }))
                .Then(_ => DocumentIsProjected(ParentExamples.OneProjectedParent))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryProjectsOne(
                    p => p.Version.Equals(0),
                    p => new TestProjection
                    {
                        Id = p.Id,
                        Version = p.Version
                    }))
                .Then(_ => DocumentNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectMany()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryProjectsMany(
                    p => p.Version.Equals(1),
                    p => new TestProjection
                    {
                        Id = p.Id,
                        Version = p.Version
                    }))
                .Then(_ => DocumentsAreProjected(ParentExamples.AllProjectedParents))
                .BDDfy();
        }

        [Fact]
        public void ProjectManyNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryProjectsMany(
                    p => p.Version.Equals(0),
                    p => new TestProjection
                    {
                        Id = p.Id,
                        Version = p.Version
                    }))
                .Then(_ => DocumentsNotProjected())
                .BDDfy();
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryIsUpdatedBy(ParentExamples.OneUpdatedParent))
                .Then(_ => DocumentIsUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateByFieldSelector()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryIsUpdatedBy(ParentExamples.OneParent, p => p.Name,
                    ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentIsUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateByPredicateFieldSelector()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryIsUpdatedBy(p => p.Version.Equals(1), p => p.Name,
                    ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentIsUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateByPredicateFieldSelectorNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryIsUpdatedBy(p => p.Version.Equals(0), p => p.Name,
                    ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentNotUpdated())
                .BDDfy();
        }

        #endregion

        #region Delete

        [Fact]
        public void DeleteOneByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryDeletesOne(ParentExamples.OneParent))
                .Then(_ => RepositoryDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryDeletesOne(ParentExamples.OneParent.Id))
                .Then(_ => RepositoryDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryDeletesOne(p => p.Version.Equals(1)))
                .Then(_ => RepositoryDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => RepositoryDeletesOne(p => p.Version.Equals(0)))
                .Then(_ => RepositoryDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByDocuments()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryDeletesMany(ParentExamples.AllParents))
                .Then(_ => RepositoryDeleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryDeletesMany(p => p.Version.Equals(1)))
                .Then(_ => RepositoryDeleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => RepositoryDeletesMany(p => p.Version.Equals(0)))
                .Then(_ => RepositoryDeleted(0))
                .And(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        #endregion
    }
}