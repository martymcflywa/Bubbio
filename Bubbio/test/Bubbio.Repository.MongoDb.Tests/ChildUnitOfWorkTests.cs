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
    public class ChildUnitOfWorkTests : UnitOfWorkTestsBase<Child, Guid>
    {
        private static readonly MongoUrl Url = new MongoUrl(TestConstants.MongoUrl);
        private static readonly IMongoDbRepository MongoDb = new MongoDbRepository(Url);
        private static readonly IRepository<Child, Guid> Repository =
            new Repository<Child, Guid>(MongoDb, Partitions.Children.ToString());
        private static readonly IUnitOfWork<Child, Guid> UnitOfWork = new ChildUnitOfWork(Repository);

        public ChildUnitOfWorkTests()
            : base(Repository, UnitOfWork)
        {
        }

        [Fact]
        public void SaveOneChild()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => SaveOne(ChildExamples.OneChild))
                .Then(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void SaveManyChildren()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => SaveMany(ChildExamples.AllChildren))
                .Then(_ => RepositoryHas(ChildExamples.AllChildren.Count()))
                .BDDfy();
        }

        [Fact]
        public void GetOneChildById()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => GetOne(ChildExamples.OneChild))
                .Then(_ => DocumentIsFound(ChildExamples.OneChild))
                .BDDfy();
        }

        [Fact]
        public void GetOneChildByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => GetOne(Guid.Empty))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneChildByDocument()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => GetOne(ChildExamples.OneChild.Id))
                .Then(_ => DocumentIsFound(ChildExamples.OneChild))
                .BDDfy();
        }

        [Fact]
        public void GetOneChildByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => GetOne(ChildExamples.AllChildren.Last()))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneChildByPredicate()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => GetOne(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentIsFound(ChildExamples.OneChild))
                .BDDfy();
        }

        [Fact]
        public void GetOneChildByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => GetOne(c => c.Version.Equals(0)))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetOneChildByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => GetOne(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetManyChildren()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => GetMany(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsAreFound(ChildExamples.AllChildren))
                .BDDfy();
        }

        [Fact]
        public void GetManyChildrenNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => GetMany(c => c.Version.Equals(0)))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void GetManyPagedChildren()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => GetManyPaged(c => c.Version.Equals(Schema.Version), 0, 1))
                .Then(_ => DocumentsAreFound(ChildExamples.AllChildren.Take(1)))
                .BDDfy();
        }

        [Fact]
        public void GetManyPagedParentsNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => GetManyPaged(c => c.Version.Equals(0), 0, 1))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Fact]
        public void CountChildren()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => Count())
                .Then(_ => DocumentsCounted(ChildExamples.AllChildren.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountChildrenEmpty()
        {
            this.Given(_ => RepositoryIsEmpty())
                .When(_ => Count())
                .Then(_ => DocumentsCounted(0))
                .BDDfy();
        }

        [Fact]
        public void CountChildrenByPredicate()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => Count(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsCounted(ChildExamples.AllChildren.Count()))
                .BDDfy();
        }

        [Fact]
        public void CountChildrenByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => Count(c => c.Version.Equals(0)))
                .Then(_ => DocumentsCounted(0))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneChild()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => ProjectOne(c => c.Version.Equals(Schema.Version), c => new TestProjection
                {
                    Id = c.Id,
                    Version = c.Version
                }))
                .Then(_ => DocumentProjected(ChildExamples.OneProjectedChild))
                .BDDfy();
        }

        [Fact]
        public void ProjectOneChildNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => ProjectOne(c => c.Version.Equals(0), c => new TestProjection
                {
                    Id = c.Id,
                    Version = c.Version
                }))
                .Then(_ => DocumentNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectOneChildManyFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => ProjectOne(c => c.Version.Equals(Schema.Version), c => new TestProjection
                {
                    Id = c.Id,
                    Version = c.Version
                }))
                .Then(_ => DocumentNotProjected())
                .BDDfy();
        }

        [Fact]
        public void ProjectManyChildren()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => ProjectMany(c => c.Version.Equals(Schema.Version), c => new TestProjection
                {
                    Id = c.Id,
                    Version = c.Version
                }))
                .Then(_ => DocumentsProjected(ChildExamples.AllProjectedChildren))
                .BDDfy();
        }

        [Fact]
        public void ProjectManyChildrenNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => ProjectMany(c => c.Version.Equals(0), c => new TestProjection
                {
                    Id = c.Id,
                    Version = c.Version
                }))
                .Then(_ => DocumentsNotProjected())
                .BDDfy();
        }

        [Fact]
        public void UpdateChildByDocument()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Update(ChildExamples.OneUpdatedChild))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(ChildExamples.OneUpdatedChild))
                .BDDfy();
        }

        [Fact]
        public void UpdateChildByDocumentNotFoundUpsert()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Update(ChildExamples.AllUpdatedChildren.Last()))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(2))
                .And(_ => RepositoryHas(ChildExamples.OneChild))
                .And(_ => RepositoryHas(ChildExamples.AllUpdatedChildren.Last()))
                .BDDfy();
        }

        [Fact]
        public void UpdateChildByDocumentField()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Update(ChildExamples.OneChild, c => c.Name, ChildExamples.OneUpdatedChild.Name))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(ChildExamples.OneUpdatedChild))
                .BDDfy();
        }

        [Fact]
        public void UpdateChildByPredicate()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Update(c => c.Id.Equals(ChildExamples.OneChild.Id),
                    c => c.Name, ChildExamples.OneUpdatedChild.Name))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(ChildExamples.OneUpdatedChild))
                .BDDfy();
        }

        [Fact]
        public void UpdateChildByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Update(p => p.Id.Equals(ParentExamples.AllParents.Last().Id),
                    c => c.Name, ChildExamples.OneUpdatedChild.Name))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(ChildExamples.OneChild))
                .BDDfy();
        }

        [Fact]
        public void UpdateChildByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => Update(c => c.Version.Equals(Schema.Version),
                    c => c.Name, ChildExamples.OneUpdatedChild.Name))
                .Then(_ => DocumentUpdated(false))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildById()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Delete(ChildExamples.OneChild.Id))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildByIdNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Delete(ChildExamples.AllChildren.Last().Id))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildByDocument()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Delete(ChildExamples.OneChild))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildByDocumentNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Delete(ChildExamples.AllChildren.Last()))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildByPredicate()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Delete(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.OneChild))
                .When(_ => Delete(c => c.Version.Equals(0)))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(1))
                .BDDfy();
        }

        [Fact]
        public void DeleteOneChildByPredicateManyFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => Delete(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(ChildExamples.AllChildren.Count()))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyChildrenByDocuments()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => DeleteMany(ChildExamples.AllChildren))
                .Then(_ => DocumentsDeleted(ChildExamples.AllChildren.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyChildrenByPredicate()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => DeleteMany(c => c.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsDeleted(ChildExamples.AllChildren.Count()))
                .And(_ => RepositoryHas(0))
                .BDDfy();
        }

        [Fact]
        public void DeleteManyChildrenByPredicateNotFound()
        {
            this.Given(_ => RepositoryContains(ChildExamples.AllChildren))
                .When(_ => DeleteMany(c => c.Version.Equals(0)))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(ChildExamples.AllChildren.Count()))
                .BDDfy();
        }
    }
}