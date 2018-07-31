using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface ITransitionEvent : IEvent
    {
        Transition Transition { get; set; }
    }
}