using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IBiometricUpdate : IEvent
    {
        BiometricType BiometricType { get; set; }
        long Measurement { get; set; }
    }
}