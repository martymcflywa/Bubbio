namespace Bubbio.Core.Contracts.Events
{
    public interface IMeasureEvent : IEvent
    {
        IMeasurement Measurement { get; set; }
    }
}