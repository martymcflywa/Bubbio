using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IBiometricUpdate : IEvent
    {
        BiometricType BiometricType { get; set; }
        float Measurement { get; set; }
    }
}