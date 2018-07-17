using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts;
using Bubbio.Repository.Core.Documents.Entities;
using Bubbio.Repository.Core.Tests.Builders;

namespace Bubbio.Repository.Core.Tests.Examples
{
    public static class ParentExamples
    {
        public static List<Guid> Ids { get; }
        public static IEnumerable<Parent> AllParents { get; }
        public static Parent OneParent => AllParents.First();

        static ParentExamples()
        {
            Ids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            AllParents = new List<Parent>
            {
                new ParentBuilder()
                    .WithId(Ids[0])
                    .WithName(new Name
                    {
                        First = "Kim",
                        Middle = "Chi",
                        Last = "Ponce"
                    })
                    .Build(),
                new ParentBuilder()
                    .WithId(Ids[1])
                    .WithName(new Name
                    {
                        First = "Marie",
                        Middle = "Kristine",
                        Last = "Nguyen"
                    })
                    .Build()
            };
        }
    }
}