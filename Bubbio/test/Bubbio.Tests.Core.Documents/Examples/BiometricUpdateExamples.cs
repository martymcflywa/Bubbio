using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Documents.Builders;

namespace Bubbio.Tests.Core.Documents.Examples
{
    public static class BiometricUpdateExamples
    {
        public static IEnumerable<BiometricUpdate> AllUpdates { get; }
        public static BiometricUpdate OneUpdate => AllUpdates.First();

        static BiometricUpdateExamples()
        {
            AllUpdates = new List<BiometricUpdate>
            {
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithBiometricType(BiometricType.Height)
                    .WithMeasurement(50)
                    .Build(),
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithBiometricType(BiometricType.Weight)
                    .WithMeasurement(3000)
                    .Build(),
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithBiometricType(BiometricType.Height)
                    .WithMeasurement(100)
                    .Build(),
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithBiometricType(BiometricType.Weight)
                    .WithMeasurement(4000)
                    .Build()
            };
        }
    }
}