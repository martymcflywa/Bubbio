using System;
using System.Reflection;

namespace Bubbio.Core.Exceptions
{
    public class ManyDocumentsFoundException : Exception
    {
        public ManyDocumentsFoundException(RepositoryOperation operation, MemberInfo documentType)
            : base($"Attempting to {operation.ToString().ToLower()} multiple {documentType.Name}")
        {
        }
    }
}