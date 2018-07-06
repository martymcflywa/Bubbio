using System;
using System.Collections.Generic;
using Bubbio.Core.Contracts;
using Bubbio.Store.MongoDb.Tests.Examples;
using Bubbio.Store.MongoDb.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests
{
    public class MongoStoreTests : MongoStoreTestsBase<IParent, Guid>
    {
        private readonly ParentExamples _parentExamples;

        public MongoStoreTests()
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

        private Guid OneId =>
            _parentExamples.OneId;

        private List<Guid> AllIds =>
            _parentExamples.AllIds;

        private IParent OneParent =>
            _parentExamples.OneParent;

        private List<IParent> AllParents =>
            _parentExamples.AllParents;
    }
}