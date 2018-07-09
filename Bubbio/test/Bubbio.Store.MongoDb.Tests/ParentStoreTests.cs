using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Entities;
using Bubbio.Store.MongoDb.Tests.Examples;
using Bubbio.Store.MongoDb.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests
{
    public class ParentStoreTests : StoreTestsBase<Parent, Guid>
    {
        private const string CollectionName = "parentstore";
        private readonly ParentExamples _parentExamples;

        private Parent OneParent =>
            _parentExamples.OneParent;

        private List<Parent> AllParents =>
            _parentExamples.AllParents;

        public ParentStoreTests()
            : base(CollectionName)
        {
            _parentExamples = new ParentExamples();
        }

        [Fact]
        public void InsertOne()
        {
            this.Given(_ => StoreIsEmpty())
                .When(_ => StoreInserts(OneParent))
                .Then(_ => StoreHas(1))
                .BDDfy();
        }

        [Fact]
        public void InsertMany()
        {
            this.Given(_ => StoreIsEmpty())
                .When(_ => StoreInserts(AllParents))
                .Then(_ => StoreHas(AllParents.Count))
                .BDDfy();
        }

        [Fact]
        public void GetOneById()
        {
            this.Given(_ => StoreContains(OneParent))
                .When(_ => StoreRetrievesOne(OneParent.Id))
                .Then(_ => StoreHas(OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByEntity()
        {
            this.Given(_ => StoreContains(OneParent))
                .When(_ => StoreRetrievesOne(OneParent))
                .Then(_ => StoreHas(OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicate()
        {
            this.Given(_ => StoreContains(OneParent))
                .When(_ => StoreRetrievesOne(p => p.Id.Equals(OneParent.Id) &&
                                                  p.GetType() == OneParent.GetType()))
                .Then(_ => StoreHas(OneParent))
                .BDDfy();
        }

        [Fact]
        public void GetManyByPredicate()
        {
            this.Given(_ => StoreContains(AllParents))
                .When(_ => StoreRetrievesMany(p => p.Id.Equals(AllParents.First().Id) ||
                                                   p.Id.Equals(AllParents.Last().Id)))
                .Then(_ => StoreHas(AllParents))
                .BDDfy();
        }
    }
}