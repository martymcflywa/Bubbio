namespace Bubbio.Core.Contracts.Events
{
    public interface IBottleFeedFormula : IBottleFeedEvent
    {
        int Scoops { get; set; }
    }
}