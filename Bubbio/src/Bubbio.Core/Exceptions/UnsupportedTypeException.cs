using System;
using System.Reflection;

namespace Bubbio.Core.Exceptions
{
    public class UnsupportedTypeException : Exception
    {
        public UnsupportedTypeException(MemberInfo type)
            : base($"{type.Name} is unsupported")
        {
        }
    }
}