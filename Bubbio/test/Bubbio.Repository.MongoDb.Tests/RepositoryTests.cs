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
    public class RepositoryTests : RepositoryTestsBase<Parent, Guid, TestProjection>
    {
        public RepositoryTests()
            : base(
                new MongoDbRepository(new MongoUrl(TestConstants.RepositoryUrl)),
                Partitions.Parents.ToString())
        {
        }

        #region Create

        [Fact]
        public void InsertOneParent()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => InsertOne(ParentExamples.OneParent))
                .Then(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void InsertManyParents()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => InsertMany(ParentExamples.AllParents))
                .Then(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        #endregion

        #region Read

        [Fact]
        public void GetOneParentById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(ParentExamples.OneParent.Id))
                .Then(_ => DocumentFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByIdNotFound()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => GetOne(ParentExamples.OneParent.Id))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(ParentExamples.OneParent))
                .Then(_ => DocumentFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByDocumentNotFound()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => GetOne(ParentExamples.OneParent))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(p => p.Version.Equals(-1)))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetOne(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetManyParents()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetMany(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsFound(ParentExamples.AllParents))
                .BDDfy();
        }

        [Fact]
        public void GetManyParentsNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetMany(p => p.Version.Equals(-1)))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetManyParentsPaged()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetMany(p => p.Version.Equals(Schema.Version), 0, 1))
                .Then(_ => DocumentsFound(ParentExamples.AllParents.Take(1)))
                .BDDfy();
        }

        [Fact]
        public void GetManyParentsPagedNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetMany(p => p.Version.Equals(-1), 0, 1))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetAnyParent()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Any(p => p.Version.Equals(Schema.Version)))
                .Then(_ => FoundAny(true))
                .BDDfy();
        }

        [Fact]
        public void GetAnyParentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Any(p => p.Version.Equals(-1)))
                .Then(_ => FoundAny(false))
                .BDDfy();
        }

        [Fact]
        public void CountAllParents()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Count())
                .Then(_ => Counted(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountAllParentsEmpty()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => Count())
                .Then(_ => Counted(0))
                .BDDfy();
        }

        [Fact]
        public void CountParentsByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Count(p => p.Version.Equals(Schema.Version)))
                .Then(_ => Counted(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountParentsByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Count(p => p.Version.Equals(-1)))
                .Then(_ => Counted(0))
                .BDDfy();
        }

        [Fact]
        public void CountParentsByPredicateEmpty()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => Count(p => p.Version.Equals(Schema.Version)))
                .Then(_ => Counted(0))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneParent()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => ProjectOne(p => p.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => OneProjected(ParentExamples.OneProjectedParent))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneParentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => ProjectOne(p => p.Version.Equals(-1), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => OneNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectOneParentManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectOne(p => p.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => OneNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectManyParents()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectMany(p => p.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ManyProjected(ParentExamples.AllProjectedParents))
                .BDDfy();
        }

        [Fact]
        public void ProjectManyParentsNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectMany(p => p.Version.Equals(-1), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ManyNotProjected())
                .BDDfy();
        }

        #endregion

        #region Update

        [Fact]
        public void UpdateOneParentByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOne(ParentExamples.OneUpdatedParent))
                .Then(_ => DocumentUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpsertOneParentByDocument()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => UpdateOne(ParentExamples.OneUpdatedParent))
                .Then(_ => DocumentUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateOneParentByField()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOne(ParentExamples.OneParent, f => f.Created,
                    ParentExamples.OneUpdatedParent.Created))
                .Then(_ => DocumentUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateOneParentByFieldNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents.Last()))
                .When(_ => UpdateOne(ParentExamples.OneParent, f => f.Modified,
                    ParentExamples.OneUpdatedParent.Modified))
                .Then(_ => DocumentNotUpdated())
                .BDDfy();
        }

        [Fact]
        public void UpdateOneParentByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOne(p => p.Version.Equals(Schema.Version), f => f.Created,
                    ParentExamples.OneUpdatedParent.Created))
                .Then(_ => DocumentUpdated(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateOneParentByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOne(p => p.Version.Equals(-1), f => f.Modified,
                    ParentExamples.OneUpdatedParent.Modified))
                .Then(_ => DocumentNotUpdated())
                .BDDfy();
        }

        [Fact]
        public void UpdateOneParentByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => UpdateOne(p => p.Version.Equals(Schema.Version), f => f.Modified,
                    ParentExamples.OneUpdatedParent.Modified))
                .Then(_ => DocumentNotUpdated())
                .BDDfy();
        }

        #endregion

        #region Delete

        [Fact]
        public void DeleteOneParentById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOne(ParentExamples.OneParent.Id))
                .Then(_ => Deleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOne(ParentExamples.AllParents.Last().Id))
                .Then(_ => Deleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOne(ParentExamples.OneParent))
                .Then(_ => Deleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOne(ParentExamples.AllParents.Last()))
                .Then(_ => Deleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyParentsByCollection()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteMany(ParentExamples.AllParents))
                .Then(_ => Deleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyParentsByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteMany(p => p.Version.Equals(Schema.Version)))
                .Then(_ => Deleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyParentsByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteMany(p => p.Version.Equals(-1)))
                .Then(_ => Deleted(0))
                .And(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        #endregion
    }
}