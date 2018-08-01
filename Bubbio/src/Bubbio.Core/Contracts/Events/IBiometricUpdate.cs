using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IBiometricUpdate : IMeasureEvent
    {
        BiometricType BiometricType { get; set; }
    }
}