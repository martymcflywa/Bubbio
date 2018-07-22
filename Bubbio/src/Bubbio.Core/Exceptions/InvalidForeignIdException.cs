using System;
using System.Reflection;

namespace Bubbio.Core.Exceptions
{
    public class InvalidForeignIdException<TKey> : Exception
        where TKey : IEquatable<TKey>
    {
        public InvalidForeignIdException(MemberInfo primaryType, MemberInfo foreignType)
            : base($"{primaryType.Name} has invalid foreign key for {foreignType.Name}")
        {
        }

        public InvalidForeignIdException(MemberInfo primaryType, MemberInfo foreignType, TKey foreignId)
            : base($"{primaryType.Name} is linked to {foreignType.Name} {foreignId} which does not exist")
        {
        }
    }
}