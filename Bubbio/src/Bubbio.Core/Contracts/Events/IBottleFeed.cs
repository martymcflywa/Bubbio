namespace Bubbio.Core.Contracts.Events
{
    public interface IBottleFeed : IEvent
    {
        long Amount { get; set; }
    }
}