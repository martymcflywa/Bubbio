using System;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Exceptions
{
    public class InvalidNameException : Exception
    {
        public InvalidNameException(NamePosition position, string name) :
            base($"{position.ToString()} name {name} contains invalid characters")
        {
        }
    }
}