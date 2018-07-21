using System;
using Bubbio.Core.Contracts;

namespace Bubbio.Core.Exceptions
{
    public class InvalidForeignIdException<TKey> : Exception
        where TKey : IEquatable<TKey>
    {
        public InvalidForeignIdException(Type primaryType, Type foreignType)
            : base($"{primaryType} has invalid foreign key for {foreignType}")
        {
        }
    }
}