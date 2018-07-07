using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Entities;
using Bubbio.Store.MongoDb.Tests.Builders;

namespace Bubbio.Store.MongoDb.Tests.Examples
{
    public class ParentExamples
    {
        public List<Parent> AllParents { get; }
        public Parent OneParent => AllParents.First();

        public ParentExamples()
        {
            var allIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            AllParents = new List<Parent>
            {
                new ParentBuilder()
                    .WithId(allIds.First())
                    .WithFirstName("Martin")
                    .WithMiddleName("Raymond")
                    .WithLastName("Ponce")
                    .Build(),
                new ParentBuilder()
                    .WithId(allIds.Last())
                    .WithFirstName("Kim")
                    .WithMiddleName("Chi")
                    .WithLastName("Ponce")
                    .Build()
            };
        }
    }
}