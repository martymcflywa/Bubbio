using System;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Contracts.Events
{
    public interface IEvent
    {
        Guid Id { get; set; }
        Guid ChildId { get; set; }
        DateTimeOffset Timestamp { get; set; }
        EventType EventType { get; set; }
    }
}