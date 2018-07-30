using System;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Exceptions
{
    public class InvalidTransitionEventException<TKey> : Exception
    {
        public InvalidTransitionEventException(EventType eventType, TKey id, Transition current)
            : base(
                $"{eventType.ToString()} event with id {id} " +
                $"requires a previous transition opposite to {current.ToString()}")
        {
        }
    }

    public class InvalidTransitionEventException : InvalidTransitionEventException<Guid>
    {
        public InvalidTransitionEventException(EventType eventType, Guid id, Transition current)
            : base(eventType, id, current)
        {
        }
    }
}