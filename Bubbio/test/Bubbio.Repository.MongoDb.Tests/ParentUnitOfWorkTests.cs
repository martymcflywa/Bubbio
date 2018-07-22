using System;
using System.Linq;
using Bubbio.Core.Repository;
using Bubbio.MongoDb;
using Bubbio.MongoDb.Documents.Constants;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.MongoDb.Interfaces;
using Bubbio.Repository.MongoDb.Tests.Scenarios;
using Bubbio.Tests.Core;
using Bubbio.Tests.Core.Examples;
using MongoDB.Driver;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Repository.MongoDb.Tests
{
    public class ParentUnitOfWorkTests : UnitOfWorkTestsBase<Parent, Guid>
    {
        private static readonly MongoUrl Url = new MongoUrl(TestConstants.MongoUrl);
        private static readonly IMongoDbRepository MongoDb = new MongoDbRepository(Url);
        private static readonly IRepository<Parent, Guid> Repository =
            new Repository<Parent, Guid>(MongoDb, Partitions.Parents.ToString());
        private static readonly IUnitOfWork<Parent, Guid> UnitOfWork = new UnitOfWork<Parent, Guid>(Repository);

        public ParentUnitOfWorkTests()
            : base(Repository, UnitOfWork)
        {
        }

        [Fact]
        public void SaveOneParent()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => SaveOne(ParentExamples.OneParent))
                .Then(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void SaveManyParents()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => SaveMany(ParentExamples.AllParents))
                .Then(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(ParentExamples.OneParent.Id))
                .Then(_ => DocumentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(Guid.Empty))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(ParentExamples.OneParent))
                .Then(_ => DocumentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(ParentExamples.AllParents.Last()))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentIsFound(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneParentByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => GetOne(p => p.Version.Equals(0)))
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
                .Then(_ => DocumentsAreFound(ParentExamples.AllParents))
                .BDDfy();
        }

        [Fact]
        public void GetManyParentsNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetMany(p => p.Version.Equals(0)))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetManyPagedParents()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetManyPaged(p => p.Version.Equals(Schema.Version), 0, 1))
                .Then(_ => DocumentsAreFound(ParentExamples.AllParents.Take(1)))
                .BDDfy();
        }

        [Fact]
        public void GetManyPagedParentsNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => GetManyPaged(p => p.Version.Equals(0), 0, 1))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void CountParents()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Count())
                .Then(_ => DocumentsCounted(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountParentsEmpty()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => Count())
                .Then(_ => DocumentsCounted(0))
                .BDDfy();
        }

        [Fact]
        public void CountParentsByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Count(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsCounted(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountParentsByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Count(p => p.Version.Equals(0)))
                .Then(_ => DocumentsCounted(0))
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
                .Then(_ => DocumentProjected(ParentExamples.OneProjectedParent))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneParentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => ProjectOne(p => p.Version.Equals(0), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => DocumentNotProjected())
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
                .Then(_ => DocumentNotProjected())
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
                .Then(_ => DocumentsProjected(ParentExamples.AllProjectedParents))
                .BDDfy();
        }

        [Fact]
        public void ProjectManyParentsNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => ProjectMany(p => p.Version.Equals(0), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => DocumentsNotProjected())
                .BDDfy();
        }

        [Fact]
        public void UpdateParentByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Update(ParentExamples.OneUpdatedParent))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateParentByDocumentNotFoundUpsert()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Update(ParentExamples.AllUpdatedParents.Last()))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(2))
                .And(_ => RepositoryHas(ParentExamples.OneParent))
                .And(_ => RepositoryHas(ParentExamples.AllUpdatedParents.Last()))
                .BDDfy();
        }

        [Fact]
        public void UpdateParentByDocumentField()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Update(ParentExamples.OneParent, p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateParentByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Update(p => p.Id.Equals(ParentExamples.OneParent.Id),
                    p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(ParentExamples.OneUpdatedParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateParentByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Update(p => p.Id.Equals(ParentExamples.AllParents.Last().Id),
                    p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(ParentExamples.OneParent))
                .BDDfy();
        }

        [Fact]
        public void UpdateParentByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Update(p => p.Version.Equals(Schema.Version),
                    p => p.Name, ParentExamples.OneUpdatedParent.Name))
                .Then(_ => DocumentUpdated(false))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentById()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Delete(ParentExamples.OneParent.Id))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Delete(ParentExamples.AllParents.Last().Id))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByDocument()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Delete(ParentExamples.OneParent))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Delete(ParentExamples.AllParents.Last()))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Delete(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.OneParent))
                .When(_ => Delete(p => p.Version.Equals(0)))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneParentByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => Delete(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyParentsByDocuments()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteMany(ParentExamples.AllParents))
                .Then(_ => DocumentsDeleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyParentsByPredicate()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteMany(p => p.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsDeleted(ParentExamples.AllParents.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyParentsByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ParentExamples.AllParents))
                .When(_ => DeleteMany(p => p.Version.Equals(0)))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(ParentExamples.AllParents.Count()))
                .BDDfy();
        }
    }
}