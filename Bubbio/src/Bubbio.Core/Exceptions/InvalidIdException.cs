using System;
using Bubbio.Core.Contracts;

namespace Bubbio.Core.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException(IName name)
            : base($"Entity named ${name.ToString()} does not have a valid id")
        {
        }
    }
}