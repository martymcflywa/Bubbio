﻿using System;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Repository.Core.Documents.Events;

namespace Bubbio.Repository.Core.Tests.Builders
{
    public class BiometricUpdateBuilder
    {
        private readonly BiometricUpdate _biometricUpdate;

        public BiometricUpdateBuilder()
        {
            _biometricUpdate = new BiometricUpdate
            {
                ChildId = Guid.NewGuid(),
                EventType = EventType.BiometricUpdate,
                BiometricType = BiometricType.Height,
                Measurement = 600
            };
        }

        public BiometricUpdateBuilder WithChildId(Guid childId)
        {
            _biometricUpdate.ChildId = childId;
            return this;
        }

        public BiometricUpdateBuilder WithBiometricType(BiometricType type)
        {
            _biometricUpdate.BiometricType = type;
            return this;
        }

        public BiometricUpdateBuilder WithMeasurement(float measurement)
        {
            _biometricUpdate.Measurement = measurement;
            return this;
        }

        public BiometricUpdate Build()
        {
            return _biometricUpdate;
        }
    }
}