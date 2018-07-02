using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IBreastFeed : IEvent
    {
        Transition Transition { get; set; }
        Side Side { get; set; }
    }
}