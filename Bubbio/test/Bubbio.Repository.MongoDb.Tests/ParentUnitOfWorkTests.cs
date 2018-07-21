using System;
using System.Linq;
using Bubbio.MongoDb;
using Bubbio.MongoDb.Documents.Constants;
using Bubbio.Repository.MongoDb.Tests.Scenarios;
using Bubbio.Tests.Core.Examples;
using MongoDB.Driver;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Repository.MongoDb.Tests
{
    public class ParentUnitOfWorkTests : ParentUnitOfWorkTestsBase
    {
        private static readonly MongoUrl Url = new MongoUrl("mongodb://localhost/test");

        public ParentUnitOfWorkTests()
            : base(new MongoDbRepository(Url))
        {
        }

        [Fact]
        public void Save()
        {
            var parent = ParentExamples.OneParent;

            this.Given(_ => RepositoryIsEmpty())
                .When(_ => ParentIsSaved(parent.Name.First, parent.Name.Last, parent.Name.Middle))
                .Then(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void SaveMany()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => ParentsAreSaved(ParentExamples.AllParents))
                .Then(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void GetOneById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOneParentById(ParentExamples.OneParent.Id))
                .Then(_ => ParentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOneParentById(Guid.Empty))
                .Then(_ => ParentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOneParentByDocument(ParentExamples.OneParent))
                .Then(_ => ParentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOneParentByDocument(ParentExamples.AllParents.Last()))
                .Then(_ => ParentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOneParentByPredicate(p => p.Version.Equals(Schema.Version)))
                .Then(_ => ParentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOneParentByPredicate(p => p.Version.Equals(0)))
                .Then(_ => ParentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetOneParentByPredicate(p => p.Version.Equals(Schema.Version)))
                .Then(_ => ParentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetMany()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetManyParents(p => p.Version.Equals(Schema.Version)))
                .Then(_ => ParentsAreFound(ParentExamples.AllParents))
                .BDDfy();
        }

        [Fact]
        public void GetManyNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetManyParents(p => p.Version.Equals(0)))
                .Then(_ => ParentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetManyPaged()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetManyParentsPaged(p => p.Version.Equals(Schema.Version), 0, 1))
                .Then(_ => ParentsAreFound(ParentExamples.AllParents.Take(1)))
                .BDDfy();
        }

        [Fact]
        public void GetManyPagedNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetManyParentsPaged(p => p.Version.Equals(0), 0, 1))
                .Then(_ => ParentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void Count()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => CountAllParents())
                .Then(_ => CountedParents(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountEmpty()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => CountAllParents())
                .Then(_ => CountedParents(0))
                .BDDfy();
        }

        [Fact]
        public void CountByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => CountParentsByPredicate(p => p.Version.Equals(Schema.Version)))
                .Then(_ => CountedParents(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => CountParentsByPredicate(p => p.Version.Equals(0)))
                .Then(_ => CountedParents(0))
                .BDDfy();
        }

        [Fact]
        public void ProjectOne()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => ProjectOneParent(p => p.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ParentIsProjected(ParentExamples.OneProjectedParent))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => ProjectOneParent(p => p.Version.Equals(0), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ParentNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectOneManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectOneParent(p => p.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ParentNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectMany()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectManyParents(p => p.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ParentsAreProjected(ParentExamples.AllProjectedParents))
                .BDDfy();
        }

        [Fact]
        public void ProjectManyNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectManyParents(p => p.Version.Equals(0), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => ParentsNotProjected())
                .BDDfy();
        }

        [Fact]
        public void UpdateByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOneParentByDocument(ParentExamples.OneUpdatedParent))
                .Then(_ => ParentIsUpdated(true))
                .And(_ => RepositoryHas(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOneParentByDocument(ParentExamples.AllUpdatedParents.Last()))
                .Then(_ => ParentIsUpdated(false))
                .And(_ => RepositoryHas(ParentExamples.AllUpdatedParents.Last()))
                .BDDfy();
        }

        [Fact]
        public void UpdateByDocumentField()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOneParentByFieldSelector(ParentExamples.OneParent, p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => ParentIsUpdated(true))
                .And(_ => RepositoryHas(p => p.Id.Equals(ParentExamples.OneUpdatedParent.Id)))
                .BDDfy();
        }

        [Fact]
        public void UpdateByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOneParentByPredicateFieldSelector(p => p.Id.Equals(ParentExamples.OneParent.Id),
                    p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => ParentIsUpdated(true))
                .And(_ => RepositoryHas(p => p.Id.Equals(ParentExamples.OneUpdatedParent.Id)))
                .BDDfy();
        }

        [Fact]
        public void UpdateByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => UpdateOneParentByPredicateFieldSelector(p => p.Id.Equals(ParentExamples.AllParents.Last().Id),
                    p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => ParentIsUpdated(false))
                .And(_ => ParentNotUpdated())
                .BDDfy();
        }

        [Fact]
        public void UpdateByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => UpdateOneParentByPredicateFieldSelector(p => p.Version.Equals(Schema.Version),
                    p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => ParentIsUpdated(false))
                .And(_ => ParentNotUpdated())
                .BDDfy();
        }

        [Fact]
        public void DeleteOneById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOneParentById(ParentExamples.OneParent.Id))
                .Then(_ => ParentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOneParentById(ParentExamples.AllParents.Last().Id))
                .Then(_ => ParentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOneParentByDocument(ParentExamples.OneParent))
                .Then(_ => ParentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOneParentByDocument(ParentExamples.AllParents.Last()))
                .Then(_ => ParentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOneParentByPredicate(p => p.Version.Equals(Schema.Version)))
                .Then(_ => ParentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => DeleteOneParentByPredicate(p => p.Version.Equals(0)))
                .Then(_ => ParentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteOneParentByPredicate(p => p.Version.Equals(Schema.Version)))
                .Then(_ => ParentsDeleted(0))
                .And(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByDocuments()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteManyParentsByDocument(ParentExamples.AllParents))
                .Then(_ => ParentsDeleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteManyParentsByPredicate(p => p.Version.Equals(Schema.Version)))
                .Then(_ => ParentsDeleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteManyParentsByPredicate(p => p.Version.Equals(0)))
                .Then(_ => ParentsDeleted(0))
                .And(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }
    }
}