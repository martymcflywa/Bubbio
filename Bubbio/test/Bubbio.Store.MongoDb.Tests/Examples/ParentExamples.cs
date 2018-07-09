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
            AllParents = new List<Parent>
            {
                new ParentBuilder()
                    .WithFirstName("Martin")
                    .WithMiddleName("Raymond")
                    .WithLastName("Ponce")
                    .Build(),
                new ParentBuilder()
                    .WithFirstName("Kim")
                    .WithMiddleName("Chi")
                    .WithLastName("Ponce")
                    .Build()
            };
        }
    }
}