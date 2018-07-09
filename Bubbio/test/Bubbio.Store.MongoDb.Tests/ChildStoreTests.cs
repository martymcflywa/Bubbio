using System;
using System.Collections.Generic;
using Bubbio.Store.MongoDb.Entities;
using Bubbio.Store.MongoDb.Tests.Examples;
using Bubbio.Store.MongoDb.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests
{
    public class ChildStoreTests : StoreTestsBase<Child, Guid>
    {
        private const string CollectionName = "childstore";
        private readonly ChildExamples _childExamples;

        private Child OneChild =>
            _childExamples.OneChild;

        private List<Child> AllChildren =>
            _childExamples.AllChildren;

        public ChildStoreTests()
            : base(CollectionName)
        {
            _childExamples = new ChildExamples();
        }

        [Fact]
        public void InsertOne()
        {
            this.Given(_ => StoreIsEmpty())
                .When(_ => StoreInserts(OneChild))
                .Then(_ => StoreHas(1))
                .BDDfy();
        }

        [Fact]
        public void InsertMany()
        {
            this.Given(_ => StoreIsEmpty())
                .When(_ => StoreInserts(AllChildren))
                .Then(_ => StoreHas(AllChildren.Count))
                .BDDfy();
        }

        [Fact]
        public void GetOneById()
        {
            this.Given(_ => StoreContains(OneChild))
                .When(_ => StoreRetrievesOne(OneChild.Id))
                .Then(_ => StoreHas(OneChild))
                .BDDfy();
        }

        [Fact]
        public void GetOneByEntity()
        {
            this.Given(_ => StoreContains(OneChild))
                .When(_ => StoreRetrievesOne(OneChild))
                .Then(_ => StoreHas(OneChild))
                .BDDfy();
        }

        [Fact]
        public void GetOneByPredicate()
        {
            this.Given(_ => StoreContains(OneChild))
                .When(_ => StoreRetrievesOne(c => c.Id.Equals(OneChild.Id)))
                .Then(_ => StoreHas(OneChild))
                .BDDfy();
        }

        [Fact]
        public void GetManyByPredicate()
        {
            this.Given(_ => StoreContains(AllChildren))
                .When(_ => StoreRetrievesMany(c => c.IsActive.Equals(true)))
                .Then(_ => StoreHas(AllChildren))
                .BDDfy();
        }
    }
}