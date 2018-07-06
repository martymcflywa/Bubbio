﻿using System;
using System.Collections.Generic;
using Bubbio.Core.Contracts;
using Bubbio.Store.MongoDb.Entities;
using Bubbio.Store.MongoDb.Tests.Examples;
using Bubbio.Store.MongoDb.Tests.Scenarios;
using TestStack.BDDfy;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests
{
    public class MongoStoreTests : MongoStoreTestsBase<ParentEntity, Guid>
    {
        private readonly ParentEntityExamples _parentEntityExamples;

        public MongoStoreTests()
        {
            _parentEntityExamples = new ParentEntityExamples();
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
            _parentEntityExamples.OneId;

        private List<Guid> AllIds =>
            _parentEntityExamples.AllIds;

        private ParentEntity OneParent =>
            _parentEntityExamples.OneParent;

        private List<ParentEntity> AllParents =>
            _parentEntityExamples.AllParents;
    }
}