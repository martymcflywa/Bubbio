using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Constants;
using Bubbio.Repository.MongoDb.Tests.Scenarios;
using Bubbio.Repository.MongoDb.Tests.Theories;
using Bubbio.Tests.Core.Examples;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Repository.MongoDb.Tests
{
    public class UnitOfWorkTests : UnitOfWorkTestsBase
    {
        #region Create

        [Theory]
        [ClassData(typeof(DocumentToInsert))]
        public void InsertOneDocument(IDocument<Guid> existing, IDocument<Guid> toInsert)
        {
            this.Given(_ => RepositoryContains(existing))
                .When(_ => InsertOne(toInsert))
                .Then(_ => RepositoryHas(toInsert))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(OrphanDocumentToInsert))]
        public void InsertOneOrphanDocument(IDocument<Guid> toInsert)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => InsertOne(toInsert))
                .Then(_ => RepositoryHas(toInsert.GetType(), 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToInsert))]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public void InsertManyDocuments(IEnumerable<IDocument<Guid>> existing, IEnumerable<IDocument<Guid>> toInsert)
        {
            this.Given(_ => RepositoryContains(existing))
                .When(_ => InsertMany(toInsert))
                .Then(_ => RepositoryHas(toInsert.First().GetType(), toInsert.Count()))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(OrphanDocumentsToInsert))]
        public void InsertManyOrphanDocuments(IEnumerable<IDocument<Guid>> toInsert)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => InsertMany(toInsert))
                .Then(_ => RepositoryHas(toInsert.First().GetType(), 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(TransitionToInsert))]
        public void InsertTransitionEvent(IEnumerable<IDocument<Guid>> existing, IDocument<Guid> toInsert,
            long expected)
        {
            this.Given(_ => RepositoryContains(existing))
                .When(_ => InsertOne(toInsert))
                .Then(_ => RepositoryHas(toInsert.GetType(), expected))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(TransitionsToInsert))]
        public void InsertTransitionEvents(IEnumerable<IDocument<Guid>> existing, IEnumerable<IDocument<Guid>> toInsert,
            long expected)
        {
            this.Given(_ => RepositoryContains(existing))
                .When(_ => InsertMany(toInsert))
                .Then(_ => RepositoryHas(toInsert.First().GetType(), expected))
                .BDDfy();
        }

        #endregion

        #region Read

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void AnyDocument(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => Any(document.GetType(), d => d.Version.Equals(Schema.Version)))
                .Then(_ => AnyFound(true))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void AnyDocumentNotFound(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => Any(document.GetType(), d => d.Version.Equals(-1)))
                .Then(_ => AnyFound(false))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void GetDocumentById(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => GetOne(document.GetType(), document.Id))
                .Then(_ => DocumentFound(document))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void GetDocumentByIdNotFound(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => GetOne(document.GetType(), Guid.Empty))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void GetDocumentByDocument(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => GetOne(document))
                .Then(_ => DocumentFound(document))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void GetDocumentByDocumentNotFound(IDocument<Guid> document)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => GetOne(document))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void GetDocumentByPredicate(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => GetOne(document.GetType(), d => d.Version.Equals(Schema.Version)))
                .Then(_ => DocumentFound(document))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToGet))]
        public void GetDocumentByPredicateNotFound(IDocument<Guid> document)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => GetOne(document.GetType(), d => d.Version.Equals(-1)))
                .When(_ => DocumentNotFound())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void GetDocumentByPredicateManyFound(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => GetOne(documents.First().GetType(), d => d.Version.Equals(Schema.Version)))
                .Then(_ => DocumentNotFound())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void GetDocuments(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => GetMany(documents.First().GetType(), d => d.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsFound(documents))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void GetDocumentsNotFound(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => GetMany(documents.First().GetType(), d => d.Version.Equals(-1)))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void GetDocumentsPaged(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => GetManyPaged(documents.First().GetType(), d => d.Version.Equals(Schema.Version), 0, 1))
                .Then(_ => DocumentsFound(documents.Take(1)))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void GetDocumentsPagedNotFound(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => GetManyPaged(documents.First().GetType(), d => d.Version.Equals(-1), 0, 1))
                .Then(_ => DocumentsNotFound())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void CountAllDocuments(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => Count(documents.First().GetType()))
                .Then(_ => DocumentsCounted(documents.Count()))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void CountAllDocumentsEmpty(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => Count(documents.First().GetType()))
                .Then(_ => DocumentsCounted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void CountDocumentsByPredicate(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => Count(documents.First().GetType(), d => d.Version.Equals(Schema.Version)))
                .Then(_ => DocumentsCounted(documents.Count()))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToGet))]
        public void CountDocumentsByPredicateNotFound(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => Count(documents.First().GetType(), d => d.Version.Equals(-1)))
                .Then(_ => DocumentsCounted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToProject))]
        public void ProjectOneDocument(IDocument<Guid> document, TestProjection projected)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => ProjectOne(document.GetType(), d => d.Version.Equals(Schema.Version), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => DocumentProjected(projected))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToProject))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void ProjectOneDocumentNotFound(IDocument<Guid> document, TestProjection projection)
        {
            this.Given(_ => RepositoryContains(document))
                .When(_ => ProjectOne(document.GetType(), d => d.Version.Equals(-1), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => DocumentNotProjected())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToProject))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void ProjectOneDocumentManyFound(IEnumerable<IDocument<Guid>> documents,
            IEnumerable<TestProjection> projections)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => ProjectOne(documents.First().GetType(), d => d.Version.Equals(Schema.Version), p =>
                    new TestProjection
                    {
                        Id = p.Id,
                        Version = p.Version
                    }))
                .Then(_ => DocumentNotProjected())
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToProject))]
        public void ProjectManyDocuments(IEnumerable<IDocument<Guid>> documents,
            IEnumerable<TestProjection> projections)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => ProjectMany(documents.First().GetType(), d => d.Version.Equals(Schema.Version), p =>
                    new TestProjection
                    {
                        Id = p.Id,
                        Version = p.Version
                    }))
                .Then(_ => DocumentsProjected(projections))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToProject))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void ProjectManyDocumentsNotFound(IEnumerable<IDocument<Guid>> documents,
            IEnumerable<TestProjection> projections)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => ProjectMany(documents.First().GetType(), d => d.Version.Equals(-1), p => new TestProjection
                {
                    Id = p.Id,
                    Version = p.Version
                }))
                .Then(_ => DocumentsNotProjected())
                .BDDfy();
        }

        #endregion

        #region Update

        [Theory]
        [ClassData(typeof(DocumentToUpdate))]
        public void UpdateDocumentByDocument(IDocument<Guid> original, IDocument<Guid> updated)
        {
            this.Given(_ => RepositoryContains(original))
                .When(_ => UpdateOne(updated))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(updated))
                .And(_ => ModifiedUpdated(original))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToUpdate))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void UpsertDocument(IDocument<Guid> original, IDocument<Guid> update)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => UpdateOne(update))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(update))
                .And(_ => ModifiedNotUpdated(update))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToUpdate))]
        public void UpdateDocumentByField(IDocument<Guid> original, IDocument<Guid> update)
        {
            var value = update.Created;

            this.Given(_ => RepositoryContains(original))
                .When(_ => UpdateOne(original, u => u.Created, value))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(update))
                .And(_ => ModifiedUpdated(update))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToUpdate))]
        public void UpdateDocumentByFieldNotFound(IDocument<Guid> original, IDocument<Guid> update)
        {
            var value = update.Created;

            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => UpdateOne(original, u => u.Created, value))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(original.GetType(), 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToUpdate))]
        public void UpdateDocumentByPredicate(IDocument<Guid> original, IDocument<Guid> update)
        {
            var value = update.Created;

            this.Given(_ => RepositoryContains(original))
                .When(_ => UpdateOne(original.GetType(), d => d.Version.Equals(Schema.Version), u => u.Created, value))
                .Then(_ => DocumentUpdated(true))
                .And(_ => RepositoryHas(update))
                .And(_ => ModifiedUpdated(update))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToUpdate))]
        public void UpdateDocumentByPredicateNotFound(IDocument<Guid> original, IDocument<Guid> update)
        {
            var value = update.Modified;

            this.Given(_ => RepositoryContains(original))
                .When(_ => UpdateOne(original.GetType(), d => d.Version.Equals(-1), u => u.Modified, value))
                .Then(_ => DocumentUpdated(false))
                .And(_ => RepositoryHas(original))
                .And(_ => ModifiedNotUpdated(original))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToUpdate))]
        public void UpdateDocumentByPredicateManyFound(IEnumerable<IDocument<Guid>> originals,
            IEnumerable<IDocument<Guid>> updates)
        {
            var value = updates.First().Modified;

            this.Given(_ => RepositoryContains(originals))
                .When(_ => UpdateOne(originals.First().GetType(), d => d.Version.Equals(Schema.Version),
                    u => u.Modified, value))
                .Then(_ => DocumentUpdated(false))
                .BDDfy();
        }

        #endregion

        #region Delete

        [Theory]
        [ClassData(typeof(DocumentToDelete))]
        public void DeleteDocumentById(IDocument<Guid> toDelete)
        {
            var type = toDelete.GetType();
            var id = toDelete.Id;
            this.Given(_ => RepositoryContains(toDelete))
                .When(_ => DeleteOne(type, id, default))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(type, 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToDelete))]
        public void DeleteDocumentByIdNotFound(IDocument<Guid> toDelete)
        {
            var type = toDelete.GetType();
            var id = toDelete.Id;
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => DeleteOne(type, id, default))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToDeleteCascade))]
        public void DeleteDocumentByIdCascade(IEnumerable<IDocument<Guid>> existing,
            IDocument<Guid> toDelete, long expected)
        {
            var type = toDelete.GetType();
            var id = toDelete.Id;
            this.Given(_ => RepositoryContains(existing))
                .When(_ => DeleteOne(type, id, true))
                .Then(_ => DocumentsDeleted(expected))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToDelete))]
        public void DeleteDocumentByDocument(IDocument<Guid> toDelete)
        {
            this.Given(_ => RepositoryContains(toDelete))
                .When(_ => DeleteOne(toDelete, false))
                .Then(_ => DocumentsDeleted(1))
                .And(_ => RepositoryHas(toDelete.GetType(), 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToDelete))]
        public void DeleteDocumentByDocumentNotFound(IDocument<Guid> toDelete)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => DeleteOne(toDelete, false))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToDeleteCascade))]
        public void DeleteDocumentByDocumentCascade(IEnumerable<IDocument<Guid>> existing, IDocument<Guid> toDelete,
            long expected)
        {
            this.Given(_ => RepositoryContains(existing))
                .When(_ => DeleteOne(toDelete, true))
                .Then(_ => DocumentsDeleted(expected))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentToDeleteCascade))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void DeleteDocumentByDocumentCascadeEmpty(IEnumerable<IDocument<Guid>> existing,
            IDocument<Guid> toDelete, long expected)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => DeleteOne(toDelete, true))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDelete))]
        public void DeleteDocumentsByDocument(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoryContains(documents))
                .When(_ => DeleteMany(documents, false))
                .Then(_ => DocumentsDeleted(documents.Count()))
                .And(_ => RepositoryHas(documents.First().GetType(), 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDelete))]
        public void DeleteDocumentsByDocumentNotFound(IEnumerable<IDocument<Guid>> documents)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => DeleteMany(documents, false))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDeleteCascade))]
        public void DeleteDocumentsByDocumentCascade(IEnumerable<IEnumerable<IDocument<Guid>>> existing,
            IEnumerable<IDocument<Guid>> toDelete, long expected)
        {
            this.Given(_ => RepositoryContains(existing.SelectMany(d => d)))
                .When(_ => DeleteMany(toDelete, true))
                .Then(_ => DocumentsDeleted(expected))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDeleteCascade))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void DeleteDocumentsByDocumentCascadeEmpty(IEnumerable<IEnumerable<IDocument<Guid>>> existing,
            IEnumerable<IDocument<Guid>> toDelete, long expected)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => DeleteMany(toDelete, true))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDelete))]
        public void DeleteDocumentsByPredicate(IEnumerable<IDocument<Guid>> documents)
        {
            var list = documents.ToList();
            var type = list.First().GetType();
            this.Given(_ => RepositoryContains(list))
                .When(_ => DeleteMany(type, d => d.Version.Equals(Schema.Version), false))
                .Then(_ => DocumentsDeleted(list.Count))
                .And(_ => RepositoryHas(type, 0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDelete))]
        public void DeleteDocumentsByPredicateNotFound(IEnumerable<IDocument<Guid>> documents)
        {
            var list = documents.ToList();
            var type = list.First().GetType();
            this.Given(_ => RepositoryContains(list))
                .When(_ => DeleteMany(type, d => d.Version.Equals(-1), false))
                .Then(_ => DocumentsDeleted(0))
                .And(_ => RepositoryHas(type, list.Count))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDeleteCascade))]
        public void DeleteDocumentsByPredicateCascade(IEnumerable<IEnumerable<IDocument<Guid>>> existing,
            IEnumerable<IDocument<Guid>> toDelete, long expected)
        {
            this.Given(_ => RepositoryContains(existing.SelectMany(d => d)))
                .When(_ => DeleteMany(toDelete.First().GetType(), d => d.Version.Equals(Schema.Version), true))
                .Then(_ => DocumentsDeleted(expected))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDeleteCascade))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void DeleteDocumentsByPredicateCascadeNotFound(IEnumerable<IEnumerable<IDocument<Guid>>> existing,
            IEnumerable<IDocument<Guid>> toDelete, long expected)
        {
            this.Given(_ => RepositoryContains(existing.SelectMany(d => d)))
                .When(_ => DeleteMany(toDelete.First().GetType(), d => d.Version.Equals(-1), true))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        [Theory]
        [ClassData(typeof(DocumentsToDeleteCascade))]
        [SuppressMessage("ReSharper", "xUnit1026")]
        public void DeleteDocumentsByPredicateCascadeEmpty(IEnumerable<IEnumerable<IDocument<Guid>>> existing,
            IEnumerable<IDocument<Guid>> toDelete, long expected)
        {
            this.Given(_ => RepositoriesAreEmpty())
                .When(_ => DeleteMany(toDelete.First().GetType(), d => d.Version.Equals(Schema.Version), true))
                .Then(_ => DocumentsDeleted(0))
                .BDDfy();
        }

        #endregion
    }
}