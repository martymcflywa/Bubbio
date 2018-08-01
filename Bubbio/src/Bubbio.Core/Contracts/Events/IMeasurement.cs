using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IMeasurement
    {
        UnitType UnitType { get; set; }
        float Amount { get; set; }
    }
}