using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts.Enums;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core.Builders;

namespace Bubbio.Tests.Core.Examples
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
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Centimetre,
                        Amount = 50
                    })
                    .Build(),
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithBiometricType(BiometricType.Weight)
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Gram,
                        Amount = 3500
                    })
                    .Build(),
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[0])
                    .WithBiometricType(BiometricType.Height)
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Centimetre,
                        Amount = 55
                    })
                    .Build(),
                new BiometricUpdateBuilder()
                    .WithChildId(ChildExamples.Ids[1])
                    .WithBiometricType(BiometricType.Weight)
                    .WithMeasurement(new Measurement
                    {
                        UnitType = UnitType.Gram,
                        Amount = 4000
                    })
                    .Build()
            };
        }
    }
}