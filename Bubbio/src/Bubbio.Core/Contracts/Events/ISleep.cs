using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface ISleep : IEvent
    {
        Transition Transition { get; set; }
    }
}