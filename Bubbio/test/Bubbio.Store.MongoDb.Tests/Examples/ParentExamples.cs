using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts;
using Bubbio.Tests.Core.Builders;

namespace Bubbio.Store.MongoDb.Tests.Examples
{
    public class ParentExamples
    {
        public List<Guid> AllIds { get; }
        public List<IParent> AllParents { get; }

        public ParentExamples()
        {
            AllIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            AllParents = new List<IParent>
            {
                new ParentBuilder()
                    .WithId(AllIds.First())
                    .WithFirstName("Martin")
                    .WithMiddleName("Raymond")
                    .WithLastName("Ponce")
                    .Build(),
                new ParentBuilder()
                    .WithId(AllIds.Last())
                    .WithFirstName("Kim")
                    .WithMiddleName("Chi")
                    .WithLastName("Ponce")
                    .Build()
            };
        }

        public Guid OneId => AllIds.First();
        public IParent OneParent => AllParents.First();
    }
}