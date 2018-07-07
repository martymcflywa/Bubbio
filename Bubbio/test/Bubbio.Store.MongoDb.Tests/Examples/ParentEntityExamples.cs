using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Store.MongoDb.Entities;
using Bubbio.Store.MongoDb.Tests.Builders;

namespace Bubbio.Store.MongoDb.Tests.Examples
{
    public class ParentEntityExamples
    {
        public List<Guid> AllIds { get; }
        public List<Parent> AllParents { get; }

        public ParentEntityExamples()
        {
            AllIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            AllParents = new List<Parent>
            {
                new ParentEntityBuilder()
                    .WithId(AllIds.First())
                    .WithFirstName("Martin")
                    .WithMiddleName("Raymond")
                    .WithLastName("Ponce")
                    .Build(),
                new ParentEntityBuilder()
                    .WithId(AllIds.Last())
                    .WithFirstName("Kim")
                    .WithMiddleName("Chi")
                    .WithLastName("Ponce")
                    .Build()
            };
        }

        public Guid OneId => AllIds.First();
        public Parent OneParent => AllParents.First();
    }
}