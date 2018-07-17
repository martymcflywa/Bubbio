namespace Bubbio.Core.Contracts.Events
{
    public interface IBottleFeed : IEvent
    {
        float Amount { get; set; }
    }
}