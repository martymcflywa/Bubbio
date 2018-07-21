using System;

namespace Bubbio.Core.Exceptions
{
    public class ManyDocumentsFoundException : Exception
    {
        public ManyDocumentsFoundException(RepositoryOperation operation, Type documentType)
            : base($"Attempting to {operation.ToString().ToLower()} multiple {documentType}")
        {
        }
    }
}